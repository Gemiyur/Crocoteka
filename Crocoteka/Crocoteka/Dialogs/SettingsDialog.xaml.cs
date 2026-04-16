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
        SaveMainWindowLocationCheckBox.IsChecked = Properties.Settings.Default.SaveMainWindowLocation;
        SaveInfoWindowsLocationCheckBox.IsChecked = Properties.Settings.Default.SaveInfoWindowsLocation;
        SaveFindFilesWindowLocationCheckBox.IsChecked = Properties.Settings.Default.SaveFindFilesWindowLocation;
        SaveNotInLibraryStateCheckBox.IsChecked = Properties.Settings.Default.SaveNotInLibraryState;
        CascadeGenreDeleteCheckBox.IsChecked = Properties.Settings.Default.CascadeGenreDelete;
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
        CascadeGenreDeleteCheckBox.IsChecked = Properties.Settings.Default.PresetCascadeGenreDelete;
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

        // Состояние флажка "Нет в библиотеке".
        Properties.Settings.Default.SaveNotInLibraryState = SaveNotInLibraryStateCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveNotInLibraryState)
        {
            Properties.Settings.Default.NotInLibraryChecked = Properties.Settings.Default.PresetNotInLibraryChecked;
        }

        // Позиция и размер главного окна.
        Properties.Settings.Default.SaveMainWindowLocation = SaveMainWindowLocationCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveMainWindowLocation)
        {
            Properties.Settings.Default.MainWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.MainWindowSize = new System.Drawing.Size(0, 0);
        }

        // Позиции и размеры окон "Об авторе". "О книге" и "О серии".
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

        // Позиция и размер окна поиска файлов.
        Properties.Settings.Default.SaveFindFilesWindowLocation = SaveFindFilesWindowLocationCheckBox.IsChecked == true;
        if (!Properties.Settings.Default.SaveFindFilesWindowLocation)
        {
            Properties.Settings.Default.FindFilesWindowPos = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.FindFilesWindowSize = new System.Drawing.Size(0, 0);
        }

        // Каскадное удаление жанров.
        Properties.Settings.Default.CascadeGenreDelete = CascadeGenreDeleteCheckBox.IsChecked == true;

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
