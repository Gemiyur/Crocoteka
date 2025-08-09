using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gemiyur.Collections;
using Crocoteka.Dialogs;
using Crocoteka.Models;
using Crocoteka.Tools;

namespace Crocoteka;

/// <summary>
/// Класс главного окна.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Коллекция отображаемых книг.
    /// </summary>
    private readonly ObservableCollectionEx<Book> ShownBooks = [];

    /// <summary>
    /// Коллекция авторов.
    /// </summary>
    private readonly ObservableCollectionEx<Author> Authors = [];

    /// <summary>
    /// Коллекция авторов.
    /// </summary>
    private readonly ObservableCollectionEx<Cycle> Cycles = [];

    /// <summary>
    /// Коллекция авторов.
    /// </summary>
    private readonly ObservableCollectionEx<Genre> Genres = [];

    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        App.DbName = Properties.Settings.Default.DebugDbName;
#else
        App.DbName = Properties.Settings.Default.DbName;
#endif
        if (!File.Exists(App.DbName))
        {
            Db.GenerateTestDb();
        }
        Authors.AddRange(Db.GetAuthors());
        Cycles.AddRange(Db.GetCycles());
        Genres.AddRange(Db.GetGenres());
        ShownBooks.AddRange(Db.GetBooks());

    }

    #region Обработчики событий окна.

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_Closed(object sender, EventArgs e)
    {

    }

    #endregion

    #region Обработчики команд группы "Библиотека".

    private void AddBook_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void FindBooks_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Authors_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Cycles_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Genres_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void CheckLibrary_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Settings_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    #endregion

    #region Обработчики команд группы "Книга".

    private void Info_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        //e.CanExecute = BooksListBox != null && BooksListBox.SelectedItem != null;
        if (!IsVisible)
            return;
        var bitmap = App.GetBitmapImage(
            e.CanExecute ? @"Images\Buttons\Enabled\Info.png" : @"Images\Buttons\Disabled\Info.png");
        ((Image)InfoButton.Content).Source = bitmap;
        ((Image)InfoMenuItem.Icon).Source = bitmap;
        //((Image)InfoContextMenuItem.Icon).Source = bitmap;
    }

    private void Info_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        //e.CanExecute = BooksListBox != null && BooksListBox.SelectedItem != null;
        if (!IsVisible)
            return;
        var bitmap = App.GetBitmapImage(
            e.CanExecute ? @"Images\Buttons\Enabled\Edit.png" : @"Images\Buttons\Disabled\Edit.png");
        ((Image)EditButton.Content).Source = bitmap;
        ((Image)EditMenuItem.Icon).Source = bitmap;
        //((Image)EditContextMenuItem.Icon).Source = bitmap;
    }

    private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        //e.CanExecute = BooksListBox != null && BooksListBox.SelectedItem != null;
        if (!IsVisible)
            return;
        var bitmap = App.GetBitmapImage(
            e.CanExecute ? @"Images\Buttons\Enabled\Delete.png" : @"Images\Buttons\Disabled\Delete.png");
        ((Image)DeleteButton.Content).Source = bitmap;
        ((Image)DeleteMenuItem.Icon).Source = bitmap;
        //((Image)DeleteContextMenuItem.Icon).Source = bitmap;
    }

    private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    #endregion

    #region Обработчики команд группы "Справка".

    private void About_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        new AboutDialog() { Owner = this }.ShowDialog();
    }

    #endregion

    private void AllBooksToggleButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void AuthorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void CyclesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void GenresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void BooksListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {

    }

    private void BooksListBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {

    }
}