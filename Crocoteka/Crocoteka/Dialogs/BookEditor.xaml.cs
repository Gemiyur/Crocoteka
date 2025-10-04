using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Gemiyur.Collections;
using Crocoteka.Models;
using Crocoteka.Tools;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс редактора книги.
/// </summary>
public partial class BookEditor : Window
{
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
    public bool CycleNumberChanged { get; private set; }

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
    /// Были ли изменения в комментариях к фалах книги.
    /// </summary>
    private bool fileCommentsChanged;

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
        cycle = book.Cycle;
        CycleTextBox.Text = cycle != null ? cycle.Title : string.Empty;
        CyclePartTextBox.Text = book.CyclePart;
        AnnotationTextBox.Text = book.Annotation;
        genres.AddRange(book.Genres);
        SortGenres();
        GenresListBox.ItemsSource = genres;
        files.AddRange(book.Files);
        SortFiles();
        FilesListBox.ItemsSource = files;
        CheckFileNotFoundVisibility();
    }

    /// <summary>
    /// Проверяет и устанавливает видимость текста "Файл не найден".
    /// </summary>
    private void CheckFileNotFoundVisibility() =>
        FileNotFoundTextBlock.Visibility = files.Any(x => !x.Exists) ? Visibility.Visible : Visibility.Collapsed;

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

        // Номер в серии.
        int.TryParse(CyclePartTextBox.Text, NumberStyles.None, null, out var cycleNumber);
        if (book.CycleNumber != cycleNumber)
        {
            book.CycleNumber = cycleNumber;
            changed = true;
            CycleNumberChanged = true;
        }

        // Аннотация.
        if (book.Annotation != AnnotationTextBox.Text)
        {
            book.Annotation = AnnotationTextBox.Text;
            changed = true;
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
            if (fileCommentsChanged)
            {
                changed = true;
                FilesChanged = true;
            }
        }
            
        // Возврат результата: были ли внесены изменения в книгу.
        return changed;
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
    }

    private void EditAuthorButton_Click(object sender, RoutedEventArgs e)
    {
        var author = (Author)AuthorsListBox.SelectedItem;
        var editor = new AuthorEditor(author) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        if (editor.NameChanged)
            SortAuthors();
    }

    private void RemoveAuthorsButton_Click(object sender, RoutedEventArgs e)
    {
        authors.RemoveRange(AuthorsListBox.SelectedItems.Cast<Author>());
    }

    private void CycleTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        EditCycleButton.IsEnabled = CycleTextBox.Text.Length > 0;
        RemoveCycleButton.IsEnabled = CycleTextBox.Text.Length > 0;
    }

    private string oldCycleNumbers = string.Empty;

    private bool CheckCycleNumbers()
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
        if (!CheckCycleNumbers())
        {
            CycleNumbersTextBox.Text = oldCycleNumbers;
            CycleNumbersTextBox.SelectionStart = pos - 1;
        }
        else
        {
            oldCycleNumbers = CycleNumbersTextBox.Text;
        }
    }

    #region Номер книги в серии - старое.

    private string oldCyclePartText = string.Empty;

    private void CyclePartTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (oldCyclePartText == CyclePartTextBox.Text)
            return;
        var text = CyclePartTextBox.Text;
        if (text == string.Empty)
        {
            oldCyclePartText = string.Empty;
            CyclePartTextBox.Text = oldCyclePartText;
            return;
        }
        var pos = CyclePartTextBox.SelectionStart;
        if (!int.TryParse(text, NumberStyles.None, null, out var value))
        {
            CyclePartTextBox.Text = oldCyclePartText;
            CyclePartTextBox.SelectionStart = pos - 1;
        }
        else
        {
            oldCyclePartText = CyclePartTextBox.Text;
        }
    }

    #endregion

    private void PickCycleButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new CyclePicker() { Owner = this };
        if (picker.ShowDialog() != true || picker.PickedCycle == null)
            return;
        if (cycle != null && picker.PickedCycle.CycleId == cycle.CycleId)
            return;
        cycle = picker.PickedCycle;
        CycleTextBox.Text = cycle.Title;
        CyclePartTextBox.Text = string.Empty;
    }

    private void NewCycleButton_Click(object sender, RoutedEventArgs e)
    {
        var newCycle = new Cycle();
        var editor = new CycleEditor(newCycle) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        cycle = newCycle;
        CycleTextBox.Text = cycle.Title;
        CyclePartTextBox.Text = string.Empty;
    }

    private void EditCycleButton_Click(object sender, RoutedEventArgs e)
    {
        if (cycle == null)
            return;
        var editor = new CycleEditor(cycle) { Owner = this };
        if (editor.ShowDialog() != true || !editor.TitleChanged)
            return;
        CycleTextBox.Text = cycle.Title;
    }

    private void RemoveCycleButton_Click(object sender, RoutedEventArgs e)
    {
        cycle = null;
        CycleTextBox.Text = string.Empty;
        CyclePartTextBox.Text = string.Empty;
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

    private void EditGenreButton_Click(object sender, RoutedEventArgs e)
    {
        var genre = (Genre)GenresListBox.SelectedItem;
        var editor = new GenreEditor(genre) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        SortGenres();
    }

    private void RemoveGenresButton_Click(object sender, RoutedEventArgs e)
    {
        genres.RemoveRange(GenresListBox.SelectedItems.Cast<Genre>());
    }

    private void NewGenreTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        AddNewGenreButton.IsEnabled = !string.IsNullOrWhiteSpace(NewGenreTextBox.Text);
    }

    private void AddNewGenreButton_Click(object sender, RoutedEventArgs e)
    {
        var title = NewGenreTextBox.Text.Trim();
        var genre = Library.Genres.Find(x => x.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase));
        if (genre != null)
        {
            if (genres.Any(x => x.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase)))
            {
                NewGenreTextBox.Text = string.Empty;
                return;
            }
            else
            {
                genres.Add(genre);
            }
        }
        else
        {
            if (genres.Any(x => x.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase)))
            {
                NewGenreTextBox.Text = string.Empty;
                return;
            }
            else
            {
                genre = new Genre() { Title = title };
                if (!Library.AddGenre(genre))
                {
                    MessageBox.Show("Не удалось сохранить жанр.", Title);
                    NewGenreTextBox.Text = string.Empty;
                    return;
                }
                genres.Add(genre);
            }
        }
        SortGenres();
        NewGenreTextBox.Text = string.Empty;
    }

    #endregion

    #region Обработчики событий элементов вкладки "Файлы".

    private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        CommentFileButton.IsEnabled = FilesListBox.SelectedItems.Count == 1;
        RemoveFilesButton.IsEnabled = FilesListBox.SelectedItems.Count > 0;
    }

    private void CommentFileButton_Click(object sender, RoutedEventArgs e)
    {
        var file = (BookFile)FilesListBox.SelectedItem;
        var editor = new FileCommentEditor(file.Comment) { Owner = this };
        if (editor.ShowDialog() == true)
        {
            file.Comment = editor.Comment;
            fileCommentsChanged = true;
        }
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
    }

    private void RemoveFilesButton_Click(object sender, RoutedEventArgs e)
    {
        files.RemoveRange(FilesListBox.SelectedItems.Cast<BookFile>());
        CheckFileNotFoundVisibility();
    }

    #endregion

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (!SaveBook())
        {
            DialogResult = false;
            return;
        }
        var saved = book.BookId > 0 ? Library.UpdateBook(book) : Library.AddBook(book);
        if (!saved)
            MessageBox.Show("Не удалось сохранить книгу в базе данных.", Title);
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();
}
