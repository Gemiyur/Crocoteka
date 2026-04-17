using System.Windows;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс окна настроек приложения.
/// </summary>
public partial class SettingsDialog : Window
{
    public SettingsDialog()
    {
        InitializeComponent();

        NavPanelAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.NavPanelAuthorFullName;
        BookListAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.BookListAuthorFullName;
        BookInfoAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.BookInfoAuthorFullName;
        CascadeGenreDeleteCheckBox.IsChecked = Properties.Settings.Default.CascadeGenreDelete;
        SaveMainWindowLocationCheckBox.IsChecked = Properties.Settings.Default.SaveMainWindowLocation;
        SaveInfoWindowsSizeCheckBox.IsChecked = Properties.Settings.Default.SaveInfoWindowsSize;
        SaveEditorsSizeCheckBox.IsChecked = Properties.Settings.Default.SaveEditorsSize;
        SaveFindFilesWindowLocationCheckBox.IsChecked = Properties.Settings.Default.SaveFindFilesWindowLocation;
        SaveNotInLibraryStateCheckBox.IsChecked = Properties.Settings.Default.SaveNotInLibraryState;
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        NavPanelAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.PresetNavPanelAuthorFullName;
        BookListAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.PresetBookListAuthorFullName;
        BookInfoAuthorFullNameCheckBox.IsChecked = Properties.Settings.Default.PresetBookInfoAuthorFullName;
        CascadeGenreDeleteCheckBox.IsChecked = Properties.Settings.Default.PresetCascadeGenreDelete;
        SaveMainWindowLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveMainWindowLocation;
        SaveInfoWindowsSizeCheckBox.IsChecked = Properties.Settings.Default.PresetSaveInfoWindowsSize;
        SaveEditorsSizeCheckBox.IsChecked = Properties.Settings.Default.PresetSaveEditorsSize;
        SaveFindFilesWindowLocationCheckBox.IsChecked = Properties.Settings.Default.PresetSaveFindFilesWindowLocation;
        SaveNotInLibraryStateCheckBox.IsChecked = Properties.Settings.Default.PresetSaveNotInLibraryState;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Имена авторов.
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

        // Каскадное удаление жанров.
        Properties.Settings.Default.CascadeGenreDelete = CascadeGenreDeleteCheckBox.IsChecked == true;

        // Позиция и размер главного окна.
        Properties.Settings.Default.SaveMainWindowLocation = SaveMainWindowLocationCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveMainWindowLocation)
        {
            Properties.Settings.Default.MainWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.MainWindowSize = new System.Drawing.Size(0, 0);
        }

        // Размеры окон "Об авторе". "О книге" и "О серии".
        Properties.Settings.Default.SaveInfoWindowsSize = SaveInfoWindowsSizeCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveInfoWindowsSize)
        {
            Properties.Settings.Default.AuthorInfoWindowSize = new System.Drawing.Size(0, 0);
            Properties.Settings.Default.BookInfoWindowSize = new System.Drawing.Size(0, 0);
            Properties.Settings.Default.CycleInfoWindowSize = new System.Drawing.Size(0, 0);
        }

        // Размеры редакторов книги, автора и серии.
        Properties.Settings.Default.SaveEditorsSize = SaveEditorsSizeCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveEditorsSize)
        {
            Properties.Settings.Default.AuthorEditorSize = new System.Drawing.Size(0, 0);
            Properties.Settings.Default.BookEditorSize = new System.Drawing.Size(0, 0);
            Properties.Settings.Default.CycleEditorSize = new System.Drawing.Size(0, 0);
        }

        // Позиция и размер окна поиска файлов.
        Properties.Settings.Default.SaveFindFilesWindowLocation = SaveFindFilesWindowLocationCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveFindFilesWindowLocation)
        {
            Properties.Settings.Default.FindFilesWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.FindFilesWindowSize = new System.Drawing.Size(0, 0);
        }

        // Состояние флажка "Нет в библиотеке".
        Properties.Settings.Default.SaveNotInLibraryState = SaveNotInLibraryStateCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveNotInLibraryState)
        {
            Properties.Settings.Default.NotInLibraryChecked = Properties.Settings.Default.PresetNotInLibraryChecked;
        }

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
