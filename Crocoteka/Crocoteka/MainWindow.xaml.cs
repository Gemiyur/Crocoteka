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
            //Db.GenerateTestDb();
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
}