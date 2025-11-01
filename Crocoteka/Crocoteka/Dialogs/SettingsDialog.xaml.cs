using System.IO;
using System.Windows;
using System.Windows.Controls;
using Gemiyur.Collections;
using Crocoteka.Tools;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс окна настроек приложения.
/// </summary>
public partial class SettingsDialog : Window
{
    private readonly ObservableCollectionEx<string> AudioExts = [];

    private readonly ObservableCollectionEx<string> TextExts = [];

    private readonly char[] invalidChars = Path.GetInvalidFileNameChars();

    private bool DbNameChanged =>
        !DbNameTextBox.Text.Equals(App.DbName, StringComparison.CurrentCultureIgnoreCase);

    public SettingsDialog()
    {
        InitializeComponent();

        // Интерфейс.
        NavPanelAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.NavPanelAuthorFullName;
        BookListAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.BookListAuthorFullName;
        BookInfoAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.BookInfoAuthorFullName;
        NotInLibraryCheckedCheckBox.IsChecked = Properties.Settings.Default.NotInLibraryChecked;
        SaveMainWindowLocationCheckBox.IsChecked = Properties.Settings.Default.SaveMainWindowLocation;
        SaveInfoWindowsLocationCheckBox.IsChecked = Properties.Settings.Default.SaveInfoWindowsLocation;
        SaveFindFilesWindowLocationCheckBox.IsChecked = Properties.Settings.Default.SaveFindFilesWindowLocation;
        SaveNotInLibraryStateCheckBox.IsChecked = Properties.Settings.Default.SaveNotInLibraryState;

        // Расширения файлов.
        var array = Properties.Settings.Default.AudioExtensions.Split(';', StringSplitOptions.RemoveEmptyEntries);
        AudioExts.AddRange(array.Select(x => x.TrimStart('.')));
        AudioExtsListBox.ItemsSource = AudioExts;
        array = Properties.Settings.Default.TextExtensions.Split(';', StringSplitOptions.RemoveEmptyEntries);
        TextExts.AddRange(array.Select(x => x.TrimStart('.')));
        TextExtsListBox.ItemsSource = TextExts;

        // База данных.
#if DEBUG
        DbNameTextBox.Text = Properties.Settings.Default.DebugDbName;
#else
        DbNameTextBox.Text = Properties.Settings.Default.DbName;
#endif
        CheckDbNameChanged();
    }

    private void CheckDbNameChanged()
    {
        DbChangedStackPanel.Visibility = DbNameChanged ? Visibility.Visible : Visibility.Collapsed;
        DbNotChangedStackPanel.Visibility = DbNameChanged ? Visibility.Collapsed : Visibility.Visible;
        DbShrinkButton.IsEnabled = !DbNameChanged;
    }

    private void ResetInterface()
    {
        NavPanelAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.PresetNavPanelAuthorFullName;
        BookListAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.PresetBookListAuthorFullName;
        BookInfoAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.PresetBookInfoAuthorFullName;
        NotInLibraryCheckedCheckBox.IsChecked = Properties.Settings.Default.PresetNotInLibraryChecked;
        SaveMainWindowLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveMainWindowLocation;
        SaveInfoWindowsLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveInfoWindowsLocation;
        SaveFindFilesWindowLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveFindFilesWindowLocation;
        SaveNotInLibraryStateCheckBox.IsChecked = Properties.Settings.Default.PresetSaveNotInLibraryState;
    }

    private void ResetExtensions()
    {
        AudioExts.Clear();
        var array = Properties.Settings.Default.PresetAudioExtensions.Split(';', StringSplitOptions.RemoveEmptyEntries);
        AudioExts.AddRange(array.Select(x => x.TrimStart('.')));
        TextExts.Clear();
        array = Properties.Settings.Default.PresetTextExtensions.Split(';', StringSplitOptions.RemoveEmptyEntries);
        TextExts.AddRange(array.Select(x => x.TrimStart('.')));
    }

    private bool ValidateExtension(string extension)
    {
        return !extension.Any(x => invalidChars.Contains(x)) &&
               !extension.Contains(' ') &&
               !(extension.Length > 0 && extension[0] == '.');
    }

    private void SettingsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ResetButton.IsEnabled = SettingsTabControl.SelectedItem == InterfaceTabItem ||
                                SettingsTabControl.SelectedItem == ExtensionsTabItem;
    }

    private void AudioExtsListBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        if (e.OriginalSource is not TextBlock)
        {
            e.Handled = true;
        }
    }

    private void DeleteAudioExtMenuItem_Click(object sender, RoutedEventArgs e)
    {
        AudioExts.Remove((string)AudioExtsListBox.SelectedItem);
    }

    private string oldAudioExt = string.Empty;

    private void AudioExtTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (oldAudioExt == AudioExtTextBox.Text)
            return;
        var text = AudioExtTextBox.Text;
        if (text == string.Empty)
        {
            oldAudioExt = string.Empty;
            AudioExtTextBox.Text = oldAudioExt;
            AddAudioExtButton.IsEnabled = false;
            return;
        }
        var pos = AudioExtTextBox.SelectionStart;
        if (!ValidateExtension(AudioExtTextBox.Text))
        {
            AudioExtTextBox.Text = oldAudioExt;
            AudioExtTextBox.SelectionStart = pos - 1;
        }
        else
        {
            oldAudioExt = AudioExtTextBox.Text;
        }
        AddAudioExtButton.IsEnabled = AudioExtTextBox.Text.Length > 0;
    }

    private void AddAudioExtButton_Click(object sender, RoutedEventArgs e)
    {
        var ext = AudioExtTextBox.Text.ToLower();
        if (AudioExts.Contains(ext))
        {
            MessageBox.Show($"Расширение \"{ext}\" уже есть.", Title);
            return;
        }
        AudioExts.Add(ext);
        AudioExts.Sort(x => x, StringComparer.CurrentCultureIgnoreCase);
        AudioExtTextBox.Clear();
    }

    private void TextExtsListBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        if (e.OriginalSource is not TextBlock)
        {
            e.Handled = true;
        }
    }

    private void DeleteTextExtMenuItem_Click(object sender, RoutedEventArgs e)
    {
        TextExts.Remove((string)TextExtsListBox.SelectedItem);
    }

    private string oldTextExt = string.Empty;

    private void TextExtTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (oldTextExt == TextExtTextBox.Text)
            return;
        var text = TextExtTextBox.Text;
        if (text == string.Empty)
        {
            oldTextExt = string.Empty;
            TextExtTextBox.Text = oldTextExt;
            AddTextExtButton.IsEnabled = false;
            return;
        }
        var pos = TextExtTextBox.SelectionStart;
        if (!ValidateExtension(TextExtTextBox.Text))
        {
            TextExtTextBox.Text = oldTextExt;
            TextExtTextBox.SelectionStart = pos - 1;
        }
        else
        {
            oldTextExt = TextExtTextBox.Text;
        }
        AddTextExtButton.IsEnabled = TextExtTextBox.Text.Length > 0;
    }

    private void AddTextExtButton_Click(object sender, RoutedEventArgs e)
    {
        var ext = TextExtTextBox.Text.ToLower();
        if (TextExts.Contains(ext))
        {
            MessageBox.Show($"Расширение \"{ext}\" уже есть.", Title);
            return;
        }
        TextExts.Add(ext);
        TextExts.Sort(x => x, StringComparer.CurrentCultureIgnoreCase);
        TextExtTextBox.Clear();
    }

    private void DbShrinkButton_Click(object sender, RoutedEventArgs e)
    {
        if (!App.ConfirmAction("Сжать базу данных библиотеки?", Title))
            return;
        var path = Path.GetDirectoryName(App.DbName) ?? "";
        var name = Path.GetFileNameWithoutExtension(App.DbName);
        var ext = Path.GetExtension(App.DbName);
        var filename = Path.Combine(path, name + "-backup" + ext);
        try { File.Delete(filename); }
        catch { }
        Db.Shrink();
        MessageBox.Show("Сжатие базы данных библиотеки завершено.", Title);
    }

    private void DbNameButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = App.PickDatabaseDialog;
        if (dialog.ShowDialog() != true)
            return;
        var dbName = Db.EnsureDbExtension(dialog.FileName);
        if (!Db.ValidateDb(dbName))
        {
            MessageBox.Show("Файл не является базой данных LiteDB или повреждён.", Title);
            return;
        }
        DbNameTextBox.Text = dbName;
        CheckDbNameChanged();
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        if (SettingsTabControl.SelectedItem == InterfaceTabItem)
            ResetInterface();
        else if (SettingsTabControl.SelectedItem == ExtensionsTabItem)
            ResetExtensions();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Интерфейс имена авторов.
        var mainWindow = App.GetMainWindow();
        var isChecked = NavPanelAuthorFullNameCheckBox.IsChecked == true;
        if (Properties.Settings.Default.NavPanelAuthorFullName != isChecked)
        {
            Properties.Settings.Default.NavPanelAuthorFullName = isChecked;
            mainWindow.CheckAuthorsNameFormat();
        }
        isChecked = BookListAuthorFullNameCheckBox.IsChecked == true;
        if (Properties.Settings.Default.BookListAuthorFullName != isChecked)
        {
            Properties.Settings.Default.BookListAuthorFullName = isChecked;
            mainWindow.UpdateShownBooks();
        }
        Properties.Settings.Default.BookInfoAuthorFullName = BookInfoAuthorFullNameCheckBox.IsChecked == true;

        // Интерфейс - состояние флажка "Нет в библиотеке".
        Properties.Settings.Default.NotInLibraryChecked = NotInLibraryCheckedCheckBox.IsChecked == true;
        Properties.Settings.Default.SaveNotInLibraryState = SaveNotInLibraryStateCheckBox.IsChecked == true;

        // Интерфейс - позиция и размер главного окна.
        Properties.Settings.Default.SaveMainWindowLocation = SaveMainWindowLocationCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveMainWindowLocation)
        {
            Properties.Settings.Default.MainWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.MainWindowSize = new System.Drawing.Size(0, 0);
        }

        // Интерфейс - позиции и размеры окон "Об авторе". "О книге" и "О серии".
        Properties.Settings.Default.SaveInfoWindowsLocation = SaveInfoWindowsLocationCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveInfoWindowsLocation)
        {
            Properties.Settings.Default.AuthorInfoWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.AuthorInfoWindowSize = new System.Drawing.Size(0, 0);
            Properties.Settings.Default.BookInfoWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.BookInfoWindowSize = new System.Drawing.Size(0, 0);
            Properties.Settings.Default.CycleInfoWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.CycleInfoWindowSize = new System.Drawing.Size(0, 0);
        }

        // Интерфейс - позиция и размер окна поиска файлов.
        Properties.Settings.Default.SaveFindFilesWindowLocation = SaveFindFilesWindowLocationCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveFindFilesWindowLocation)
        {
            Properties.Settings.Default.FindFilesWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.FindFilesWindowSize = new System.Drawing.Size(0, 0);
        }

        // Расширения файлов.
        var newAudioExts = AudioExts.Select(x => $".{x}");
        var newAudioExtsSetting = App.ListToString(newAudioExts, ";");
        var audioExtsChanged = Properties.Settings.Default.AudioExtensions != newAudioExtsSetting;
        if (audioExtsChanged)
        {
            Properties.Settings.Default.AudioExtensions = newAudioExtsSetting;
            App.AudioExtensions.Clear();
            App.AudioExtensions.AddRange(newAudioExts);
        }

        var newTextExts = TextExts.Select(x => $".{x}");
        var newTextExtsSetting = App.ListToString(newTextExts, ";");
        var textExtsChanged = Properties.Settings.Default.TextExtensions != newTextExtsSetting;
        if (textExtsChanged)
        {
            Properties.Settings.Default.TextExtensions = newTextExtsSetting;
            App.TextExtensions.Clear();
            App.TextExtensions.AddRange(newTextExts);
        }

        if (audioExtsChanged || textExtsChanged)
            mainWindow.UpdateShownBooks();

        // База данных.
#if DEBUG
        Properties.Settings.Default.DebugDbName = DbNameTextBox.Text;
#else
        Properties.Settings.Default.DbName = DbNameTextBox.Text;
#endif

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
