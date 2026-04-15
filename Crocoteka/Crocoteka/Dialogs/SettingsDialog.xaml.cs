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
        SaveInfoWindowsLocationCheckBox.IsChecked = Properties.Settings.Default.SaveInfoWindowsLocation;
        SaveFindFilesWindowLocationCheckBox.IsChecked = Properties.Settings.Default.SaveFindFilesWindowLocation;
        SaveNotInLibraryStateCheckBox.IsChecked = Properties.Settings.Default.SaveNotInLibraryState;

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

    private void SettingsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
        ResetButton.IsEnabled = SettingsTabControl.SelectedItem == InterfaceTabItem;

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
            MessageBox.Show("Файл не является базой данных Крокотеки или повреждён.", Title);
            return;
        }
        DbNameTextBox.Text = dbName;
        CheckDbNameChanged();
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        NavPanelAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.PresetNavPanelAuthorFullName;
        BookListAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.PresetBookListAuthorFullName;
        BookInfoAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.PresetBookInfoAuthorFullName;
        SaveMainWindowLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveMainWindowLocation;
        SaveInfoWindowsLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveInfoWindowsLocation;
        SaveFindFilesWindowLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveFindFilesWindowLocation;
        SaveNotInLibraryStateCheckBox.IsChecked = Properties.Settings.Default.PresetSaveNotInLibraryState;
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
        Properties.Settings.Default.SaveNotInLibraryState = SaveNotInLibraryStateCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveNotInLibraryState)
        {
            Properties.Settings.Default.NotInLibraryChecked = Properties.Settings.Default.PresetNotInLibraryChecked;
        }

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

        // База данных.
        if (DbNameChanged)
        {
#if DEBUG
            Properties.Settings.Default.DebugDbName = DbNameTextBox.Text;
#else
            Properties.Settings.Default.DbName = DbNameTextBox.Text;
#endif
            var newDb = !File.Exists(DbNameTextBox.Text);
            using var db = Db.GetDatabase(DbNameTextBox.Text);
            if (newDb)
                Db.InitializeCollections(db);
        }

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
