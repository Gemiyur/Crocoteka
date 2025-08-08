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

    private void AboutButton_Click(object sender, RoutedEventArgs e)
    {
        new AboutDialog() { Owner = this }.ShowDialog();
    }

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

    }

    #endregion

    #region Обработчики команд группы "Книга".

    private void Info_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {

    }

    private void Info_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {

    }

    private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {

    }

    private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    #endregion

    #region Обработчики команд группы "Справка".

    private void About_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    #endregion
}