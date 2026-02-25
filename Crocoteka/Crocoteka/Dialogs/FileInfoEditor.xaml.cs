using System.Windows;
using System.Windows.Controls;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс редактора информации о файле книги.
/// </summary>
public partial class FileInfoEditor : Window
{
    public string Comment { get; private set; }

    public FileInfoEditor(string comment)
    {
        InitializeComponent();
        Comment = comment;
        CommentTextBox.Text = comment;
    }

    private void CommentTextBox_TextChanged(object sender, TextChangedEventArgs e) =>
        SaveButton.IsEnabled = CommentTextBox.Text != Comment;

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Comment = CommentTextBox.Text;
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
