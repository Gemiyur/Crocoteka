using System.Windows;
using System.Windows.Controls;
using Gemiyur.Collections;
using Crocoteka.Models;
using Crocoteka.Tools;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс редактора жанров.
/// </summary>
public partial class GenresEditor : Window
{
    /// <summary>
    /// Были ли изменения в коллекции жанров.
    /// </summary>
    public bool HasChanges;

    /// <summary>
    /// Коллекция жанров.
    /// </summary>
    private readonly ObservableCollectionEx<Genre> genres = [];

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
    public GenresEditor()
    {
        InitializeComponent();
        genres.AddRange(Library.Genres);
        GenresListBox.ItemsSource = genres;
    }

    /// <summary>
    /// Сортирует коллекцию жанров по названию.
    /// </summary>
    private void SortGenres() => genres.Sort(x => x.Title, StringComparer.CurrentCultureIgnoreCase);

    private void GenresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EditButton.IsEnabled = GenresListBox.SelectedIndex >= 0;
        DeleteButton.IsEnabled = GenresListBox.SelectedIndex >= 0 &&
                                 !Library.GenreHasBooks(((Genre)GenresListBox.SelectedItem).GenreId);
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var genre = new Genre();
        var editor = new GenreEditor(genre) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        genres.Add(genre);
        SortGenres();
        HasChanges = true;
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var genre = (Genre)GenresListBox.SelectedItem;
        var editor = new GenreEditor(genre) { Owner = this };
        if (editor.ShowDialog() != true)
            return;
        SortGenres();
        HasChanges = true;
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var genre = (Genre)GenresListBox.SelectedItem;
        if (!Library.DeleteGenre(genre))
        {
            MessageBox.Show("Не удалось удалить жанр.", Title);
            return;
        }
        genres.Remove(genre);
        HasChanges = true;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
