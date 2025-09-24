using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Gemiyur.Collections;
using Crocoteka.Dialogs;
using Crocoteka.Models;
using Crocoteka.Tools;
using System.Globalization;

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

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
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
        App.AudioExtensions.AddRange(Properties.Settings.Default.AudioExtensions.Split(';'));
        App.TextExtensions.AddRange(Properties.Settings.Default.TextExtensions.Split(';'));
        Authors.AddRange(Library.Authors);
        AuthorsListBox.ItemsSource = Authors;
        CheckAuthorsNameFormat();
        Cycles.AddRange(Library.Cycles);
        CyclesListBox.ItemsSource = Cycles;
        Genres.AddRange(Library.Genres);
        GenresListBox.ItemsSource = Genres;
        ShownBooks.AddRange(Library.Books);
        BooksListView.ItemsSource = ShownBooks;
        UpdateStatusBarBooksCount();
    }

    /// <summary>
    /// Создаёт и добавляет новую книгу в библиотеку.
    /// </summary>
    /// <param name="files">Список файлов книги.</param>
    /// <returns>Была ли добавлена книга.</returns>
    public bool AddBook(IEnumerable<string> files)
    {
        var book = new Book();
        foreach (var file in files)
        {
            var bookFile = new BookFile() { Filename = file };
            book.Files.Add(bookFile);
        }
        if (book.Files.Count > 0)
        {
            book.Title = book.Files[0].NameOnly;
        }
        var editor = new BookEditor(book) { Owner = this };
        var saved = editor.ShowDialog() == true;
        UpdateNavPanel(true, true, true);
        if (saved)
        {
            UpdateShownBooks();
            SelectBookInShownBooks(book);
        }
        return saved;
    }

    /// <summary>
    /// Устанавливает формат отображения имён авторов в панели навигации.
    /// </summary>
    public void CheckAuthorsNameFormat()
    {
        AuthorsListBox.ItemTemplate = Properties.Settings.Default.NavAuthorFullName
            ? (DataTemplate)FindResource("AuthorFullNameDataTemplate")
            : (DataTemplate)FindResource("AuthorShortNameDataTemplate");
    }

    /// <summary>
    /// Выделяет указанную книгу в списке отображаемых книг.
    /// </summary>
    /// <param name="book">Книга.</param>
    /// <remarks>Если указанной книги в списке нет, то ничего не делает.</remarks>
    private void SelectBookInShownBooks(Book book)
    {
        if (ShownBooks.Contains(book))
        {
            BooksListView.SelectedItem = book;
            BooksListView.ScrollIntoView(BooksListView.SelectedItem);
        }
    }

    /// <summary>
    /// Отображает окно информации о книге.
    /// </summary>
    /// <param name="book">Книга.</param>
    public void ShowBookInfo(Book book)
    {
        //var window = App.FindBookInfoWindow();
        //if (window != null)
        //{
        //    if (window.Book != book)
        //        window.Book = book;
        //    App.RestoreWindow(window);
        //    window.Activate();
        //}
        //else
        //{
        //    new BookInfoDialog(book) { Owner = this }.Show();
        //}
    }

    /// <summary>
    /// Обновляет списки панели навигации.
    /// </summary>
    /// <param name="authors">Обновить список авторов.</param>
    /// <param name="cycles">Обновить список серий.</param>
    /// <param name="genres">Обновить список жанров.</param>
    private void UpdateNavPanel(bool authors, bool cycles, bool genres)
    {
        LockNavHandlers();
        if (authors)
        {
            var selectedAuthor = (Author)AuthorsListBox.SelectedItem;
            Authors.ReplaceRange(Library.Authors);
            if (selectedAuthor != null)
            {
                AuthorsListBox.SelectedItem = Authors.FirstOrDefault(x => x.AuthorId == selectedAuthor.AuthorId);
                if (AuthorsListBox.SelectedItem != null)
                    AuthorsListBox.ScrollIntoView(AuthorsListBox.SelectedItem);
            }
        }
        if (cycles)
        {
            var selectedCycle = (Cycle)CyclesListBox.SelectedItem;
            Cycles.ReplaceRange(Library.Cycles);
            if (selectedCycle != null)
            {
                CyclesListBox.SelectedItem = Cycles.FirstOrDefault(x => x.CycleId == selectedCycle.CycleId);
                if (CyclesListBox.SelectedItem != null)
                    CyclesListBox.ScrollIntoView(CyclesListBox.SelectedItem);
            }
        }
        if (genres)
        {
            var selectedGenre = (Genre)GenresListBox.SelectedItem;
            Genres.ReplaceRange(Library.Genres);
            if (selectedGenre != null)
            {
                GenresListBox.SelectedItem = Genres.FirstOrDefault(x => x.Equals(selectedGenre));
                if (GenresListBox.SelectedItem != null)
                    GenresListBox.ScrollIntoView(GenresListBox.SelectedItem);
            }
        }
        if (AuthorsListBox.SelectedItem == null &&
            CyclesListBox.SelectedItem == null &&
            GenresListBox.SelectedItem == null &&
            AllBooksToggleButton.IsChecked != true)
        {
            AllBooksToggleButton.IsChecked = true;
        }
        UnlockNavHandlers();
    }

    /// <summary>
    /// Обновляет список отображаемых книг.
    /// </summary>
    private void UpdateShownBooks()
    {
        if (AllBooksToggleButton.IsChecked == true)
        {
            ShownBooks.ReplaceRange(Library.Books);
            BooksListViewTitleColumn.CellTemplate = (DataTemplate)FindResource("BookDataTemplate");
        }
        else if (AuthorsListBox.SelectedItem != null)
        {
            var author = (Author)AuthorsListBox.SelectedItem;
            var books = Library.GetAuthorBooks(author.AuthorId);
            ShownBooks.ReplaceRange(books);
            BooksListViewTitleColumn.CellTemplate = (DataTemplate)FindResource("BookDataTemplate");
        }
        else if (CyclesListBox.SelectedItem != null)
        {
            var cycle = (Cycle)CyclesListBox.SelectedItem;
            var books = Library.GetCycleBooks(cycle.CycleId);
            ShownBooks.ReplaceRange(books);
            BooksListViewTitleColumn.CellTemplate = (DataTemplate)FindResource("BookCycleDataTemplate");
        }
        else if (GenresListBox.SelectedItem != null)
        {
            var tag = (Genre)GenresListBox.SelectedItem;
            var books = Library.GetGenreBooks(tag.GenreId);
            ShownBooks.ReplaceRange(books);
            BooksListViewTitleColumn.CellTemplate = (DataTemplate)FindResource("BookDataTemplate");
        }
        UpdateStatusBarBooksCount();
    }

    /// <summary>
    /// Обновляет имена авторов в списке отображаемых книг.
    /// </summary>
    private void UpdateShownBooksAuthors()
    {
        foreach (var book in ShownBooks)
        {
            book.OnPropertyChanged("AuthorNamesFirstLast");
            book.OnPropertyChanged("AuthorNamesFirstMiddleLast");
            book.OnPropertyChanged("AuthorNamesLastFirst");
            book.OnPropertyChanged("AuthorNamesLastFirstMiddle");
        }
    }

    /// <summary>
    /// Обновляет количество отображаемых книг в строке статуса.
    /// </summary>
    private void UpdateStatusBarBooksCount() => BooksCountTextBlock.Text = BooksListView.Items.Count.ToString();

    #region Блокировка и разблокировка обработчиков событий элементов панели навигации.

    /// <summary>
    /// Блокируются ли обработчики событий элементов панели навигации.
    /// </summary>
    private bool NavHandlersLocked;

    /// <summary>
    /// Блокирует обработчики событий элементов панели навигации.
    /// </summary>
    private void LockNavHandlers() => NavHandlersLocked = true;

    /// <summary>
    /// Разблокирует обработчики событий элементов панели навигации.
    /// </summary>
    private void UnlockNavHandlers() => NavHandlersLocked = false;

    #endregion

    #region Обработчики событий окна.

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //if (Properties.Settings.Default.SaveMainWindowLocation &&
        //    App.SizeDefined(Properties.Settings.Default.MainWindowSize))
        //{
        //    Left = Properties.Settings.Default.MainWindowPos.X;
        //    Top = Properties.Settings.Default.MainWindowPos.Y;
        //    Width = Properties.Settings.Default.MainWindowSize.Width;
        //    Height = Properties.Settings.Default.MainWindowSize.Height;
        //}
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        //if (Properties.Settings.Default.SaveMainWindowLocation)
        //{
        //    Properties.Settings.Default.MainWindowPos = new System.Drawing.Point((int)Left, (int)Top);
        //    Properties.Settings.Default.MainWindowSize = new System.Drawing.Size((int)Width, (int)Height);
        //}
        //Properties.Settings.Default.Save();
    }

    #endregion

    #region Обработчики событий элементов панели навигации.

    private void AllBooksToggleButton_Click(object sender, RoutedEventArgs e)
    {
        if (AllBooksToggleButton.IsChecked != true)
        {
            AllBooksToggleButton.IsChecked = true;
            return;
        }
        LockNavHandlers();
        AuthorsListBox.SelectedIndex = -1;
        CyclesListBox.SelectedIndex = -1;
        GenresListBox.SelectedIndex = -1;
        UnlockNavHandlers();
        UpdateShownBooks();
    }

    private void AuthorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (NavHandlersLocked)
            return;
        AllBooksToggleButton.IsChecked = AuthorsListBox.SelectedIndex < 0;
        LockNavHandlers();
        CyclesListBox.SelectedIndex = -1;
        GenresListBox.SelectedIndex = -1;
        UnlockNavHandlers();
        UpdateShownBooks();
    }

    private void CyclesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (NavHandlersLocked)
            return;
        AllBooksToggleButton.IsChecked = CyclesListBox.SelectedIndex < 0;
        LockNavHandlers();
        AuthorsListBox.SelectedIndex = -1;
        GenresListBox.SelectedIndex = -1;
        UnlockNavHandlers();
        UpdateShownBooks();
    }

    private void GenresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (NavHandlersLocked)
            return;
        AllBooksToggleButton.IsChecked = GenresListBox.SelectedIndex < 0;
        LockNavHandlers();
        AuthorsListBox.SelectedIndex = -1;
        CyclesListBox.SelectedIndex = -1;
        UnlockNavHandlers();
        UpdateShownBooks();
    }

    #endregion

    #region Обработчики событий элемента списка книг.

    private void BooksListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        if (e.OriginalSource is not TextBlock)
        {
            e.Handled = true;
        }
    }

    private void BooksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (BooksListView.SelectedItem != null && (e.OriginalSource is TextBlock || e.OriginalSource is Border))
            ShowBookInfo((Book)BooksListView.SelectedItem);
    }

    private void BooksListView_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var listView = (ListView)sender;
        var gridView = (GridView)listView.View;
        var totalWidth = listView.ActualWidth - (SystemParameters.VerticalScrollBarWidth + 10);
        var usedWidth = 0.0;
        for (var i = 1; i < gridView.Columns.Count; i++)
        {
            usedWidth += gridView.Columns[i].Width;
        }
        gridView.Columns[0].Width = totalWidth - usedWidth;
    }

    #endregion

    #region Обработчики команд библиотеки.

    private void Settings_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    #endregion

    #region Обработчики команд книг.

    private void AddBook_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var files = new List<string>();
        var dialog = App.PickBookFileDialog;
        if (dialog.ShowDialog() == true)
            files.AddRange(dialog.FileNames);
        if (AddBook(files))
            App.UpdateFindFilesWindow();
    }

    private void FindBooks_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var window = App.GetFindFilesWindow();
        if (window != null)
        {
            App.RestoreWindow(window);
            window.Activate();
        }
        else
        {
            new FindFilesWindow().Show();
        }
    }

    private void BookInfo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = BooksListView != null && BooksListView.SelectedItem != null;
        if (!IsVisible)
            return;
        var bitmap = App.GetBitmapImage(
            e.CanExecute ? @"Images\Buttons\Enabled\BookInfo.png" : @"Images\Buttons\Disabled\BookInfo.png");
        ((Image)BookInfoButton.Content).Source = bitmap;
        ((Image)BookInfoMenuItem.Icon).Source = bitmap;
    }

    private void BookInfo_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void BookEdit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = BooksListView != null && BooksListView.SelectedItem != null;
        if (!IsVisible)
            return;
        var bitmap = App.GetBitmapImage(
            e.CanExecute ? @"Images\Buttons\Enabled\Edit.png" : @"Images\Buttons\Disabled\Edit.png");
        ((Image)BookEditButton.Content).Source = bitmap;
        ((Image)BookEditMenuItem.Icon).Source = bitmap;
    }

    private void BookEdit_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var book = (Book)BooksListView.SelectedItem;
        var editor = new BookEditor(book) { Owner = this };
        var result = editor.ShowDialog() == true;
        UpdateNavPanel(true, true, true);
        if (!result)
            return;
        if (editor.TitleChanged || editor.AuthorsChanged ||
            editor.CycleChanged || editor.CycleNumberChanged || editor.GenresChanged)
        {
            UpdateShownBooks();
            SelectBookInShownBooks(book);
        }
        if (editor.FilesChanged)
            App.UpdateFindFilesWindow();
        book.OnPropertyChanged("AuthorNamesFirstLast");
        book.OnPropertyChanged("AuthorNamesFirstMiddleLast");
        book.OnPropertyChanged("AuthorNamesLastFirst");
        book.OnPropertyChanged("AuthorNamesLastFirstMiddle");
        book.OnPropertyChanged("FilesCountText");
        book.OnPropertyChanged("AudioCountText");
        book.OnPropertyChanged("TextCountText");
        book.OnPropertyChanged("NotFoundCountText");
    }

    private void BookDelete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = BooksListView != null && BooksListView.SelectedItem != null;
        if (!IsVisible)
            return;
        var bitmap = App.GetBitmapImage(
            e.CanExecute ? @"Images\Buttons\Enabled\Delete.png" : @"Images\Buttons\Disabled\Delete.png");
        ((Image)BookDeleteButton.Content).Source = bitmap;
        ((Image)BookDeleteMenuItem.Icon).Source = bitmap;
    }

    private void BookDelete_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var book = (Book)BooksListView.SelectedItem;
        if (MessageBox.Show($"Удалить книгу \"{book.Title}\" из библиотеки?", Title,
                            MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        {
            return;
        }
        if (!Library.DeleteBook(book))
        {
            MessageBox.Show($"Не удалось удалить книгу \"{book.Title}\" из библиотеки.", Title);
            return;
        }
        UpdateShownBooks();
        App.UpdateFindFilesWindow();
    }

    #endregion

    #region Обработчики команд авторов.

    private void Authors_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var editor = new AuthorsEditor() { Owner = this };
        editor.ShowDialog();
        if (!editor.HasChanges)
            return;
        var selectedItem = AuthorsListBox.SelectedItem;
        UpdateNavPanel(true, false, false);
        if (selectedItem != null && AuthorsListBox.SelectedItem == null)
            UpdateShownBooks();
        UpdateShownBooksAuthors();
    }

    private void AuthorInfo_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void AuthorEdit_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var author = (Author)AuthorsListBox.SelectedItem;
        var editor = new AuthorEditor(author) { Owner = this };
        if (editor.ShowDialog() != true || !editor.NameChanged)
            return;
        UpdateNavPanel(true, false, false);
        UpdateShownBooksAuthors();
    }

    private void AuthorDelete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = !Library.AuthorHasBooks(((Author)AuthorsListBox.SelectedItem).AuthorId);
        if (!IsVisible)
            return;
        var bitmap = App.GetBitmapImage(
            e.CanExecute ? @"Images\Buttons\Enabled\Delete.png" : @"Images\Buttons\Disabled\Delete.png");
        ((Image)AuthorDeleteContextMenuItem.Icon).Source = bitmap;
    }

    private void AuthorDelete_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var author = (Author)AuthorsListBox.SelectedItem;
        if (!Library.DeleteAuthor(author))
        {
            MessageBox.Show("Не удалось удалить автора.", Title);
            return;
        }
        Authors.Remove(author);
        UpdateNavPanel(true, false, false);
    }

    #endregion

    #region Обработчики команд серий.

    private void Cycles_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var editor = new CyclesEditor() { Owner = this };
        editor.ShowDialog();
        if (!editor.HasChanges)
            return;
        var selectedItem = CyclesListBox.SelectedItem;
        UpdateNavPanel(false, true, false);
        if (selectedItem != null && CyclesListBox.SelectedItem == null)
            UpdateShownBooks();
    }

    private void CycleInfo_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    private void CycleEdit_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var cycle = (Cycle)CyclesListBox.SelectedItem;
        var editor = new CycleEditor(cycle) { Owner = this };
        if (editor.ShowDialog() != true || !editor.TitleChanged)
            return;
        UpdateNavPanel(false, true, false);
    }

    private void CycleDelete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = !Library.CycleHasBooks(((Cycle)CyclesListBox.SelectedItem).CycleId);
        if (!IsVisible)
            return;
        var bitmap = App.GetBitmapImage(
            e.CanExecute ? @"Images\Buttons\Enabled\Delete.png" : @"Images\Buttons\Disabled\Delete.png");
        ((Image)CycleDeleteContextMenuItem.Icon).Source = bitmap;
    }

    private void CycleDelete_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var cycle = (Cycle)CyclesListBox.SelectedItem;
        if (!Library.DeleteCycle(cycle))
        {
            MessageBox.Show("Не удалось удалить серию.", Title);
            return;
        }
        Cycles.Remove(cycle);
        UpdateNavPanel(false, true, false);
    }

    #endregion

    #region Обработчики команд жанров.

    private void Genres_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var editor = new GenresEditor() { Owner = this };
        editor.ShowDialog();
        if (!editor.HasChanges)
            return;
        var selectedItem = GenresListBox.SelectedItem;
        UpdateNavPanel(false, false, true);
        if (selectedItem != null && GenresListBox.SelectedItem == null)
            UpdateShownBooks();
    }

    private void GenreEdit_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var genre = (Genre)GenresListBox.SelectedItem;
        var editor = new GenreEditor(genre) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        UpdateNavPanel(false, false, true);
    }

    private void GenreDelete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = !Library.GenreHasBooks(((Genre)GenresListBox.SelectedItem).GenreId);
        if (!IsVisible)
            return;
        var bitmap = App.GetBitmapImage(
            e.CanExecute ? @"Images\Buttons\Enabled\Delete.png" : @"Images\Buttons\Disabled\Delete.png");
        ((Image)GenreDeleteContextMenuItem.Icon).Source = bitmap;
    }

    private void GenreDelete_Executed(object sender, ExecutedRoutedEventArgs e)
    {

    }

    #endregion

    #region Обработчики команд справки.

    private void About_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        new AboutDialog() { Owner = this }.ShowDialog();
    }

    #endregion
}