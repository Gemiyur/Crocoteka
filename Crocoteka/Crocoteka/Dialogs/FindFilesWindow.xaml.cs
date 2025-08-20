using System.IO;
using System.Windows;
using System.Windows.Controls;
using Crocoteka.Tools;
using Gemiyur.Collections;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс окна поиска файлов книг.
/// </summary>
public partial class FindFilesWindow : Window
{
    private readonly Dictionary<string, List<string>> masks = [];

    private readonly List<string> bookExtensions = [];

    private string folder = string.Empty;

    private readonly List<string> files = [];

    private readonly ObservableCollectionEx<string> shownFiles = [];

    public FindFilesWindow()
    {
        InitializeComponent();

        bookExtensions.AddRange(App.AudioExtensions);
        bookExtensions.AddRange(App.TextExtensions);
        bookExtensions.AddRange(App.ZipExtensions);

        var mask = new KeyValuePair<string, List<string>>("Все файлы книг", []);
        mask.Value.AddRange(bookExtensions);
        masks.Add(mask.Key, mask.Value);

        mask = new KeyValuePair<string, List<string>>("Аудио и текст", []);
        mask.Value.AddRange(App.AudioExtensions);
        mask.Value.AddRange(App.TextExtensions);
        masks.Add(mask.Key, mask.Value);

        mask = new KeyValuePair<string, List<string>>("Аудио", []);
        mask.Value.AddRange(App.AudioExtensions);
        masks.Add(mask.Key, mask.Value);

        mask = new KeyValuePair<string, List<string>>("Текст", []);
        mask.Value.AddRange(App.TextExtensions);
        masks.Add(mask.Key, mask.Value);

        mask = new KeyValuePair<string, List<string>>("Архивы", []);
        mask.Value.AddRange(App.ZipExtensions);
        masks.Add(mask.Key, mask.Value);

        TypeComboBox.ItemsSource = masks;
        TypeComboBox.SelectedIndex = 0;

        FilesListBox.ItemsSource = shownFiles;
    }

    private void ApplyFilter()
    {
        var extensions = ((KeyValuePair<string, List<string>>)TypeComboBox.SelectedItem).Value;
        var list = files.FindAll(x => extensions.Contains(Path.GetExtension(x), StringComparer.CurrentCultureIgnoreCase));
        if (NotInLibraryCheckBox.IsChecked == true)
            list.RemoveAll(x => Library.FileHasBooks(FullName(x)));
        shownFiles.ReplaceRange(list);
        UpdateCount();
    }

    private string FullName(string name) => Path.Combine(folder, name);

    private void LoadFiles()
    {
        files.Clear();
        var trimCount = folder.Length + 1;
        var list = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
            .Where(x => bookExtensions.Contains(Path.GetExtension(x), StringComparer.CurrentCultureIgnoreCase))
            .Select(x => x[trimCount..])
            .OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase);
        files.AddRange(list);
        ApplyFilter();
    }

    private void UpdateCount() => CountTextBlock.Text = shownFiles.Count.ToString();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //if (Properties.Settings.Default.SaveBookWindowsLocation &&
        //    App.SizeDefined(Properties.Settings.Default.BookInfoSize))
        //{
        //    Left = Properties.Settings.Default.BookInfoPos.X;
        //    Top = Properties.Settings.Default.BookInfoPos.Y;
        //    Width = Properties.Settings.Default.BookInfoSize.Width;
        //    Height = Properties.Settings.Default.BookInfoSize.Height;
        //}
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        //if (Properties.Settings.Default.SaveBookWindowsLocation)
        //{
        //    Properties.Settings.Default.BookInfoPos = new System.Drawing.Point((int)Left, (int)Top);
        //    Properties.Settings.Default.BookInfoSize = new System.Drawing.Size((int)Width, (int)Height);
        //}
        App.GetMainWindow().Activate();
    }

    private void FolderButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = App.PickBooksFolderDialog;
        if (dialog.ShowDialog() != true)
            return;

        if (dialog.FolderName.Equals(folder, StringComparison.CurrentCultureIgnoreCase))
        {
            // TODO: Что делать если папка та же? Просто обновить или спросить?
        }

        folder = dialog.FolderName;
        FolderTextBox.Text = folder;

        LoadFiles();
    }

    private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();

    private void NotInLibraryCheckBox_Click(object sender, RoutedEventArgs e) => ApplyFilter();

    private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void BookButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
