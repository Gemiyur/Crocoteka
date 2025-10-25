using System.IO;
using System.Windows;
using System.Windows.Controls;
using Gemiyur.Collections;
using Crocoteka.Tools;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс окна поиска файлов книг.
/// </summary>
public partial class FindFilesWindow : Window
{
    /// <summary>
    /// Словарь расширений файлов для типов книг.
    /// </summary>
    private readonly Dictionary<string, List<string>> masks = [];

    /// <summary>
    /// Список всех расширений файлов книг.
    /// </summary>
    private readonly List<string> bookExtensions = [];

    /// <summary>
    /// Папка с файлами книг.
    /// </summary>
    private string folder = string.Empty;

    /// <summary>
    /// Список имён файлов книг в папке без пути папки.
    /// </summary>
    /// <remarks>Файлы книг отсортированы по имени.</remarks>
    private readonly List<string> files = [];

    /// <summary>
    /// Коллекция отображаемых файлов книг.
    /// </summary>
    private readonly ObservableCollectionEx<string> shownFiles = [];

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
    public FindFilesWindow()
    {
        InitializeComponent();

        bookExtensions.AddRange(App.AudioExtensions);
        bookExtensions.AddRange(App.TextExtensions);

        var mask = new KeyValuePair<string, List<string>>("Все файлы книг", []);
        mask.Value.AddRange(bookExtensions);
        masks.Add(mask.Key, mask.Value);

        mask = new KeyValuePair<string, List<string>>("Аудио", []);
        mask.Value.AddRange(App.AudioExtensions);
        masks.Add(mask.Key, mask.Value);

        mask = new KeyValuePair<string, List<string>>("Текст", []);
        mask.Value.AddRange(App.TextExtensions);
        masks.Add(mask.Key, mask.Value);

        TypeComboBox.ItemsSource = masks;
        TypeComboBox.SelectedIndex = 0;

        NotInLibraryCheckBox.IsChecked = Properties.Settings.Default.NotInLibraryChecked;

        FilesListBox.ItemsSource = shownFiles;
    }

    /// <summary>
    /// Применяет фильтр к списку файлов книг.
    /// </summary>
    public void ApplyFilter()
    {
        var extensions = ((KeyValuePair<string, List<string>>)TypeComboBox.SelectedItem).Value;
        var list = files.FindAll(x => extensions.Contains(Path.GetExtension(x), StringComparer.CurrentCultureIgnoreCase));
        if (NotInLibraryCheckBox.IsChecked == true)
            list.RemoveAll(x => Library.FileHasBooks(FullName(x)));
        shownFiles.ReplaceRange(list);
        UpdateCount();
    }

    /// <summary>
    /// Возвращает имя файла книги с полным путём.
    /// </summary>
    /// <param name="name">Имя файла книги без пути папки.</param>
    /// <returns>Имя файла книги с полным путём.</returns>
    private string FullName(string name) => Path.Combine(folder, name);

    /// <summary>
    /// Загружает список файлов книг в папке.
    /// </summary>
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

    /// <summary>
    /// Обновляет количество файлов книг в списке файлов книг.
    /// </summary>
    private void UpdateCount() => CountTextBlock.Text = shownFiles.Count.ToString();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (Properties.Settings.Default.SaveFindFilesWindowLocation &&
            App.SizeDefined(Properties.Settings.Default.FindFilesWindowSize))
        {
            Left = Properties.Settings.Default.FindFilesWindowPos.X;
            Top = Properties.Settings.Default.FindFilesWindowPos.Y;
            Width = Properties.Settings.Default.FindFilesWindowSize.Width;
            Height = Properties.Settings.Default.FindFilesWindowSize.Height;
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (Properties.Settings.Default.SaveFindFilesWindowLocation)
        {
            Properties.Settings.Default.FindFilesWindowPos = new System.Drawing.Point((int)Left, (int)Top);
            Properties.Settings.Default.FindFilesWindowSize = new System.Drawing.Size((int)Width, (int)Height);
        }
        if (Properties.Settings.Default.SaveNotInLibraryState)
        {
            Properties.Settings.Default.NotInLibraryChecked = NotInLibraryCheckBox.IsChecked == true;
        }

        // TODO: Надо ли восстанавливать и активировать главное окно при закрытии окна поиска файлов?
        var window = App.GetMainWindow();
        if (window != null)
        {
            App.RestoreWindow(window);
            window.Activate();
        }
    }

    private void FolderButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = App.PickBooksFolderDialog;
        if (dialog.ShowDialog() != true ||
            dialog.FolderName.Equals(folder, StringComparison.CurrentCultureIgnoreCase))
        {
            return;
        }
        folder = dialog.FolderName;
        FolderTextBox.Text = folder;
        LoadFiles();
        ReloadButton.IsEnabled = true;
    }

    private void ReloadButton_Click(object sender, RoutedEventArgs e) => LoadFiles();

    private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();

    private void NotInLibraryCheckBox_Click(object sender, RoutedEventArgs e) => ApplyFilter();

    private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        BookButton.IsEnabled = FilesListBox.SelectedItems.Count > 0;
        SelectFileButton.IsEnabled = FilesListBox.SelectedItems.Count == 1;
        OpenFileButton.IsEnabled = FilesListBox.SelectedItems.Count == 1;
    }

    private void BookButton_Click(object sender, RoutedEventArgs e)
    {
        if (App.GetMainWindow().AddBook(FilesListBox.SelectedItems.Cast<string>().Select(FullName)))
            ApplyFilter();
    }

    private void SelectFileButton_Click(object sender, RoutedEventArgs e)
    {
        var filename = FullName((string)FilesListBox.SelectedItem);
        App.ShowFileInFolder(filename, Title);
    }

    private void OpenFileButton_Click(object sender, RoutedEventArgs e)
    {
        var filename = FullName((string)FilesListBox.SelectedItem);
        App.OpenFile(filename, Title);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
