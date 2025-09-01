using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Gemiyur.Collections;
using Crocoteka.Models;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс редактора книги.
/// </summary>
public partial class BookEditor : Window
{
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
    }

    private void NewAuthorButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void EditAuthorButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void RemoveAuthorsButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CycleTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        EditCycleButton.IsEnabled = CycleTextBox.Text.Length > 0;
        RemoveCycleButton.IsEnabled = CycleTextBox.Text.Length > 0;
    }

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

    private void PickCycleButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void NewCycleButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void EditCycleButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void RemoveCycleButton_Click(object sender, RoutedEventArgs e)
    {

    }

    #endregion

    #region Обработчики событий элементов вкладки "Аннотация".

    #endregion

    #region Обработчики событий элементов вкладки "Жанры".

    private void GenresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EditGenreButton.IsEnabled = GenresListBox.SelectedItems.Count == 1;
        RemoveGenresButton.IsEnabled = GenresListBox.SelectedItems.Count > 0;
    }

    private void PickGenresButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void EditGenreButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void RemoveGenresButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void NewGenreTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        AddNewGenreButton.IsEnabled = !string.IsNullOrWhiteSpace(NewGenreTextBox.Text);
    }

    private void AddNewGenreButton_Click(object sender, RoutedEventArgs e)
    {

    }

    #endregion

    #region Обработчики событий элементов вкладки "Файлы".

    private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EditFileButton.IsEnabled = FilesListBox.SelectedItems.Count == 1;
        RemoveFilesButton.IsEnabled = FilesListBox.SelectedItems.Count > 0;
    }

    private void AddFilesButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void EditFileButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void RemoveFilesButton_Click(object sender, RoutedEventArgs e)
    {

    }

    #endregion

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
