using System.IO;
using System.Windows;
using System.Windows.Controls;
using Crocoteka.Tools;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс окна настроек приложения.
/// </summary>
public partial class SettingsDialog : Window
{
    private bool DbNameChanged =>
        !DbNameTextBox.Text.Equals(App.DbName, StringComparison.CurrentCultureIgnoreCase);

    public SettingsDialog()
    {
        InitializeComponent();

        // Интерфейс.
        NavPanelAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.NavPanelAuthorFullName;
        BookListAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.BookListAuthorFullName;
        BookInfoAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.BookInfoAuthorFullName;
        SaveMainWindowLocationCheckBox.IsChecked = Properties.Settings.Default.SaveMainWindowLocation;
        SaveFindFilesWindowLocationCheckBox.IsChecked = Properties.Settings.Default.SaveFindFilesWindowLocation;
        SaveInfoWindowsLocationCheckBox.IsChecked = Properties.Settings.Default.SaveInfoWindowsLocation;

        // Расширения файлов.

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
        SaveMainWindowLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveMainWindowLocation;
        SaveFindFilesWindowLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveFindFilesWindowLocation;
        SaveInfoWindowsLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveInfoWindowsLocation;
    }

    private void ResetExtensions()
    {
        MessageBox.Show("Вызов ResetExtensions");
    }

    private void SettingsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ResetButton.IsEnabled = SettingsTabControl.SelectedItem == InterfaceTabItem ||
                                SettingsTabControl.SelectedItem == ExtensionsTabItem;
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
        DbNameTextBox.Text = App.EnsureDbExtension(dialog.FileName);
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
        Properties.Settings.Default.NavPanelAuthorFullName = NavPanelAuthorFullNameCheckBox.IsChecked == true;
        mainWindow.CheckAuthorsNameFormat();
        Properties.Settings.Default.BookListAuthorFullName = BookListAuthorFullNameCheckBox.IsChecked == true;
        mainWindow.UpdateShownBooks();
        Properties.Settings.Default.BookInfoAuthorFullName = BookInfoAuthorFullNameCheckBox.IsChecked == true;

        // Интерфейс - позиция и размер главного окна.
        Properties.Settings.Default.SaveMainWindowLocation = SaveMainWindowLocationCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveMainWindowLocation)
        {
            Properties.Settings.Default.MainWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.MainWindowSize = new System.Drawing.Size(0, 0);
        }

        // Интерфейс - позиция и размер окна поиска файлов.
        Properties.Settings.Default.SaveFindFilesWindowLocation = SaveFindFilesWindowLocationCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveFindFilesWindowLocation)
        {
            Properties.Settings.Default.FindFilesWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.FindFilesWindowSize = new System.Drawing.Size(0, 0);
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

        // Расширения файлов.

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
