using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Crocoteka.Models;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс редактора книги.
/// </summary>
public partial class BookEditor : Window
{
    public BookEditor(Book book)
    {
        InitializeComponent();
        TitleTextBox.Text = book.Title;
    }

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

    }

    private void PickCycleButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CyclePartTextBox_TextChanged(object sender, TextChangedEventArgs e)
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
