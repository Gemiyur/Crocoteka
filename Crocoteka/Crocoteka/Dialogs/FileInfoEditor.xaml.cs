using System.Windows;
using System.Windows.Controls;
using Crocoteka.Models;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс редактора информации о файле книги.
/// </summary>
public partial class FileInfoEditor : Window
{
    private readonly BookFile file;

    public FileInfoEditor(BookFile file)
    {
        InitializeComponent();
        this.file = file;
        if (!file.IsAudio)
        {
            LectorTextBlock.Visibility = Visibility.Collapsed;
            LectorTextBox.Visibility = Visibility.Collapsed;
        }
        TranslatorTextBox.Text = file.Translator;
        LectorTextBox.Text = file.Lector;
        CommentTextBox.Text = file.Comment;
    }

    private void CheckSaveButton() => SaveButton.IsEnabled =
        file.Translator != TranslatorTextBox.Text ||
        file.Lector != LectorTextBox.Text ||
        file.Comment != CommentTextBox.Text;
        

    private void TranslatorTextBox_TextChanged(object sender, TextChangedEventArgs e) => CheckSaveButton();

    private void LectorTextBox_TextChanged(object sender, TextChangedEventArgs e) => CheckSaveButton();

    private void CommentTextBox_TextChanged(object sender, TextChangedEventArgs e) => CheckSaveButton();

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        file.Translator = TranslatorTextBox.Text;
        file.Lector = LectorTextBox.Text;
        file.Comment = CommentTextBox.Text;
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
