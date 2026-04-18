using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Gemiyur.Collections;
using Crocoteka.Models;
using Crocoteka.Tools;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс редактора книги.
/// </summary>
public partial class BookEditor : Window
{
    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    private const int GWL_STYLE = -16;
    private const int WS_MAXIMIZEBOX = 0x10000;
    private const int WS_MINIMIZEBOX = 0x20000;

    /// <summary>
    /// Возвращает было ли изменено название книги.
    /// </summary>
    public bool TitleChanged {get; private set;}

    /// <summary>
    /// Возвращает были ли изменения в авторах книги.
    /// </summary>
    public bool AuthorsChanged { get; private set; }

    /// <summary>
    /// Возвращает была ли изменена серия книги.
    /// </summary>
    public bool CycleChanged { get; private set; }

    /// <summary>
    /// Возвращает был ли изменён номер книги в серии.
    /// </summary>
    public bool CycleNumbersChanged { get; private set; }

    /// <summary>
    /// Возвращает были ли изменения в жанрах книги.
    /// </summary>
    public bool GenresChanged { get; private set; }

    /// <summary>
    /// Возвращает были ли изменения в фалах книги.
    /// </summary>
    public bool FilesChanged { get; private set; }

    /// <summary>
    /// Редактируемая книга.
    /// </summary>
    private readonly Book book;

    /// <summary>
    /// Коллекция авторов книги.
    /// </summary>
    private readonly ObservableCollectionEx<Author> authors = [];

    /// <summary>
    /// Серия книги.
    /// </summary>
    private Cycle? cycle;

    /// <summary>
    /// Коллекция жанров книги.
    /// </summary>
    private readonly ObservableCollectionEx<Genre> genres = [];

    /// <summary>
    /// Коллекция файлов книги.
    /// </summary>
    private readonly ObservableCollectionEx<BookFile> files = [];

    /// <summary>
    /// Были ли изменения в информации о файлах книги.
    /// </summary>
    private bool fileInfoChanged;

    /// <summary>
    /// Инициализирует новый экземпляр класса. 
    /// </summary>
    /// <param name="book">Книга.</param>
    public BookEditor(Book book)
    {
        InitializeComponent();
        this.book = book;
        TitleTextBox.Text = book.Title;
        authors.AddRange(book.Authors);
        SortAuthors();
        AuthorsListBox.ItemsSource = authors;
        SetCycle(book.Cycle);
        CycleNumbersTextBox.Text = book.CycleNumbers;
        genres.AddRange(book.Genres);
        SortGenres();
        GenresListBox.ItemsSource = genres;
        AnnotationTextBox.Text = book.Annotation;
        NoteTextBox.Text = book.Note;
        files.AddRange(book.Files);
        SortFiles();
        FilesListBox.ItemsSource = files;
        UpdateFilesCount();
    }

    /// <summary>
    /// Сохраняет данные из редактора в редактируемую книгу.
    /// </summary>
    /// <returns>Были ли внесены изменения в книгу.</returns>
    private bool SaveBook()
    {
        // В книге есть изменения?
        var changed = false;

        // Новая книга.
        changed = book.BookId < 1;

        // Название.
        if (book.Title != TitleTextBox.Text)
        {
            book.Title = TitleTextBox.Text;
            changed = true;
            TitleChanged = true;
        }

        // Авторы.
        if (authors.Count != book.Authors.Count ||
            authors.Any(x => !book.Authors.Exists(a => a.AuthorId == x.AuthorId)) ||
            book.Authors.Any(x => !authors.Any(a => a.AuthorId == x.AuthorId)))
        {
            book.Authors.Clear();
            book.Authors.AddRange(authors);
            changed = true;
            AuthorsChanged = true;
        }

        // Серия.
        if ((cycle == null && book.Cycle != null) ||
            (cycle != null && book.Cycle == null))
        {
            book.Cycle = cycle;
            changed = true;
            CycleChanged = true;
        }
        else
        {
            if (cycle != null && book.Cycle != null &&
                cycle.CycleId != book.Cycle.CycleId)
            {
                book.Cycle = cycle;
                changed = true;
                CycleChanged = true;
            }
        }

        // Номера в серии.
        if (book.CycleNumbers != CycleNumbersTextBox.Text)
        {
            book.CycleNumbers = CycleNumbersTextBox.Text;
            changed = true;
            CycleNumbersChanged = true;
        }

        // Жанры.
        if (genres.Count != book.Genres.Count ||
            genres.Any(x => !book.Genres.Exists(g => g.GenreId == x.GenreId)) ||
            book.Genres.Any(x => !genres.Any(g => g.GenreId == x.GenreId)))
        {
            book.Genres.Clear();
            book.Genres.AddRange(genres);
            changed = true;
            GenresChanged = true;
        }

        // Аннотация.
        if (book.Annotation != AnnotationTextBox.Text)
        {
            book.Annotation = AnnotationTextBox.Text;
            changed = true;
        }

        // Примечание.
        if (book.Note != NoteTextBox.Text)
        {
            book.Note = NoteTextBox.Text;
            changed = true;
        }

        // Файлы.
        if (files.Count != book.Files.Count ||
            files.Any(x => !book.Files.Exists(f => f.Filename == x.Filename)) ||
            book.Files.Any(x => !files.Any(f => f.Filename == x.Filename)))
        {
            book.Files.Clear();
            book.Files.AddRange(files);
            changed = true;
            FilesChanged = true;
        }
        else
        {
            if (fileInfoChanged)
            {
                changed = true;
                FilesChanged = true;
            }
        }
            
        // Возврат результата: были ли внесены изменения в книгу.
        return changed;
    }

    /// <summary>
    /// Устанавливает содержимое элемента в панели информации о файле книги.
    /// </summary>
    private void SetBookFileContent()
    {
        var file = FilesListBox.SelectedItems.Count == 1 ? (BookFile)FilesListBox.SelectedItem : null;
        App.SetBookFileContent(FileInfoContentControl, file);
    }

    /// <summary>
    /// Устанавливает серию книги.
    /// </summary>
    /// <param name="value">Серия книги.</param>
    private void SetCycle(Cycle? value)
    {
        cycle = value;
        EditCycleButton.IsEnabled = cycle != null;
        RemoveCycleButton.IsEnabled = cycle != null;
        CycleNumbersTextBox.IsEnabled = cycle != null;
        CycleTextBox.Text = cycle != null ? cycle.Title : string.Empty;
    }

    /// <summary>
    /// Сортирует коллекцию авторов книги по фамилии, имени и отчеству.
    /// </summary>
    private void SortAuthors() => authors.Sort(x => x.NameLastFirstMiddle, StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    /// Сортирует коллекцию жанров книги в алфавитном порядке.
    /// </summary>
    private void SortGenres() => genres.Sort(x => x.Title, StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    /// Сортирует коллекцию файлов книги в алфавитном порядке.
    /// </summary>
    private void SortFiles() => files.Sort(x => x.Filename, StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    /// Обновляет содержимое элемента в панели информации о файле книги.
    /// </summary>
    private void UpdateBookFileContent() => App.UpdateBookFileContent(FileInfoContentControl);

    /// <summary>
    /// Обновляет отображаемое количество файлов.
    /// </summary>
    private void UpdateFilesCount()
    {
        TotalFilesTextBlock.Text = files.Count.ToString();
        AudioFilesTextBlock.Text = files.Count > 0 ? files.Count(x => x.IsAudio).ToString() : "0";
        TextFilesTextBlock.Text = files.Count > 0 ? files.Count(x => x.IsText).ToString() : "0";
        var notFoundCount = files.Count(x => !x.Exists);
        NotFoundFilesTextBlock.Text = notFoundCount.ToString();
        NotFoundFilesStackPanel.Visibility = notFoundCount > 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    #region Обработчики событий окна.

    private void Window_SourceInitialized(object sender, EventArgs e)
    {
        IntPtr handle = new WindowInteropHelper(this).Handle;
        _ = SetWindowLong(handle, GWL_STYLE, GetWindowLong(handle, GWL_STYLE) & ~WS_MINIMIZEBOX);
        _ = SetWindowLong(handle, GWL_STYLE, GetWindowLong(handle, GWL_STYLE) & ~WS_MAXIMIZEBOX);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (Properties.Settings.Default.SaveEditorsSize &&
            App.SizeDefined(Properties.Settings.Default.BookEditorSize))
        {
            Width = Properties.Settings.Default.BookEditorSize.Width;
            Height = Properties.Settings.Default.BookEditorSize.Height;
        }
        App.CenterOnScreen(this);
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (Properties.Settings.Default.SaveEditorsSize)
        {
            Properties.Settings.Default.BookEditorSize = new System.Drawing.Size((int)Width, (int)Height);
        }
    }

    #endregion

    #region Обработчики событий элементов названия книги.

    private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        SaveButton.IsEnabled = !string.IsNullOrWhiteSpace(TitleTextBox.Text);
    }

    #endregion

    #region Обработчики событий элементов вкладки "Авторы и серия".

    private void AuthorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EditAuthorButton.IsEnabled = AuthorsListBox.SelectedItems.Count == 1;
        RemoveAuthorsButton.IsEnabled = AuthorsListBox.SelectedItems.Count > 0;
    }

    private void PickAuthorsButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new AuthorsPicker() { Owner = this };
        if (picker.ShowDialog() != true)
            return;
        authors.AddRange(picker.PickedAuthors.Where(x => !authors.Any(a => a.AuthorId == x.AuthorId)));
        SortAuthors();
    }

    private void NewAuthorButton_Click(object sender, RoutedEventArgs e)
    {
        var author = new Author();
        var editor = new AuthorEditor(author) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        authors.Add(author);
        SortAuthors();
        App.GetMainWindow().UpdateNavPanel(true, false, false);
    }

    private void EditAuthorButton_Click(object sender, RoutedEventArgs e)
    {
        var author = (Author)AuthorsListBox.SelectedItem;
        var editor = new AuthorEditor(author) { Owner = this };
        if (editor.ShowDialog() != true || !editor.NameChanged)
            return;
        SortAuthors();
        App.GetMainWindow().UpdateNavPanel(true, false, false);
    }

    private void RemoveAuthorsButton_Click(object sender, RoutedEventArgs e)
    {
        authors.RemoveRange(AuthorsListBox.SelectedItems.Cast<Author>());
    }

    private string oldCycleNumbers = string.Empty;

    private bool ValidateCycleNumbers()
    {
        var array = CycleNumbersTextBox.Text.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in array)
        {
            if (!int.TryParse(item.Trim(), NumberStyles.None, null, out _))
                return false;
        }
        return true;
    }

    private void SortCycleNumbers()
    {
        var array = CycleNumbersTextBox.Text.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries);
        List<int> list = [.. array.Select(int.Parse)];
        list.Sort();
        var result = string.Empty;
        for (int i = 0; i < list.Count; i++)
        {
            if (i < list.Count - 1)
                result += $"{list[i]}, ";
            else
                result += list[i].ToString();
        }
        CycleNumbersTextBox.Text = result;
    }

    private void CycleNumbersTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        SortCycleNumbers();
    }

    private void CycleNumbersTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (oldCycleNumbers == CycleNumbersTextBox.Text)
            return;
        var text = CycleNumbersTextBox.Text;
        if (text == string.Empty)
        {
            oldCycleNumbers = string.Empty;
            CycleNumbersTextBox.Text = oldCycleNumbers;
            return;
        }
        var pos = CycleNumbersTextBox.SelectionStart;
        if (!ValidateCycleNumbers())
        {
            CycleNumbersTextBox.Text = oldCycleNumbers;
            CycleNumbersTextBox.SelectionStart = pos - 1;
        }
        else
        {
            oldCycleNumbers = CycleNumbersTextBox.Text;
        }
    }

    private void PickCycleButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new CyclePicker() { Owner = this };
        if (picker.ShowDialog() != true || picker.PickedCycle == null)
            return;
        if (cycle != null && picker.PickedCycle.CycleId == cycle.CycleId)
            return;
        SetCycle(picker.PickedCycle);
        CycleNumbersTextBox.Text = string.Empty;
    }

    private void NewCycleButton_Click(object sender, RoutedEventArgs e)
    {
        var newCycle = new Cycle();
        var editor = new CycleEditor(newCycle) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        SetCycle(newCycle);
        CycleNumbersTextBox.Text = string.Empty;
        App.GetMainWindow().UpdateNavPanel(false, true, false);
    }

    private void EditCycleButton_Click(object sender, RoutedEventArgs e)
    {
        if (cycle == null)
            return;
        var editor = new CycleEditor(cycle) { Owner = this };
        if (editor.ShowDialog() != true || !editor.TitleChanged)
            return;
        CycleTextBox.Text = cycle.Title;
        App.GetMainWindow().UpdateNavPanel(false, true, false);
    }

    private void RemoveCycleButton_Click(object sender, RoutedEventArgs e)
    {
        SetCycle(null);
        CycleNumbersTextBox.Text = string.Empty;
    }

    #endregion

    #region Обработчики событий элементов вкладки "Жанры".

    private void GenresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EditGenreButton.IsEnabled = GenresListBox.SelectedItems.Count == 1;
        RemoveGenresButton.IsEnabled = GenresListBox.SelectedItems.Count > 0;
    }

    private void PickGenresButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new GenresPicker() { Owner = this };
        if (picker.ShowDialog() != true)
            return;
        genres.AddRange(picker.PickedGenres.Where(x => !genres.Any(g => g.GenreId == x.GenreId)));
        SortGenres();
    }

    private void NewGenreButton_Click(object sender, RoutedEventArgs e)
    {
        var genre = new Genre();
        var editor = new GenreEditor(genre) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        genres.Add(genre);
        SortGenres();
        App.GetMainWindow().UpdateNavPanel(false, false, true);
    }

    private void EditGenreButton_Click(object sender, RoutedEventArgs e)
    {
        var genre = (Genre)GenresListBox.SelectedItem;
        var editor = new GenreEditor(genre) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        SortGenres();
        App.GetMainWindow().UpdateNavPanel(false, false, true);
    }

    private void RemoveGenresButton_Click(object sender, RoutedEventArgs e)
    {
        genres.RemoveRange(GenresListBox.SelectedItems.Cast<Genre>());
    }

    #endregion

    #region Обработчики событий элементов вкладки "Файлы".

    private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EditFileButton.IsEnabled = FilesListBox.SelectedItems.Count == 1;
        var fileButtonsEnabled = FilesListBox.SelectedItems.Count == 1 && ((BookFile)FilesListBox.SelectedItem).Exists;
        SelectFileButton.IsEnabled = fileButtonsEnabled;
        OpenFileButton.IsEnabled = fileButtonsEnabled;
        RemoveFilesButton.IsEnabled = FilesListBox.SelectedItems.Count > 0;
        SetBookFileContent();
    }

    private void EditFileButton_Click(object sender, RoutedEventArgs e)
    {
        var file = (BookFile)FilesListBox.SelectedItem;
        var editor = new FileInfoEditor(file) { Owner = this };
        if (editor.ShowDialog() == true)
        {
            UpdateBookFileContent();
            fileInfoChanged = true;
        }
    }

    private void SelectFileButton_Click(object sender, RoutedEventArgs e)
    {
        var filename = ((BookFile)FilesListBox.SelectedItem).Filename;
        App.ShowFileInFolder(filename, Title);
    }

    private void OpenFileButton_Click(object sender, RoutedEventArgs e)
    {
        var filename = ((BookFile)FilesListBox.SelectedItem).Filename;
        App.OpenFile(filename, Title);
    }

    private void AddFilesButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = App.PickBookFileDialog;
        if (dialog.ShowDialog() != true)
            return;
        List<string> filenames = [];
        foreach (var filename in dialog.FileNames)
        {
            if (!files.Any(x => x.Filename.Equals(filename, StringComparison.CurrentCultureIgnoreCase)))
                filenames.Add(filename);
        }
        if (filenames.Count == 0)
            return;
        foreach (var filename in filenames)
        {
            var file = new BookFile() { Filename = filename };
            files.Add(file);
        }
        SortFiles();
        UpdateFilesCount();
    }

    private void RemoveFilesButton_Click(object sender, RoutedEventArgs e)
    {
        files.RemoveRange(FilesListBox.SelectedItems.Cast<BookFile>());
        UpdateFilesCount();
    }

    #endregion

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var origBook = book.Clone();
        if (!SaveBook())
        {
            DialogResult = false;
            return;
        }
        var saved = book.BookId > 0 ? Library.UpdateBook(book) : Library.AddBook(book);
        if (!saved)
        {
            MessageBox.Show("Не удалось сохранить книгу в базе данных.", Title);
            origBook.CopyTo(book);
            DialogResult = false;
            return;
        }
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
