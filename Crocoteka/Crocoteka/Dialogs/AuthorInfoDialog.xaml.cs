using System.Windows;
using Crocoteka.Models;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс окна "Об авторе".
/// </summary>
public partial class AuthorInfoDialog : Window
{
    public AuthorInfoDialog(Author author)
    {
        InitializeComponent();
        NameTextBlock.FontSize = FontSize + 2;
        NameTextBlock.Text = author.NameFirstMiddleLast;
        AboutTextBox.Text = author.About;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_Closed(object sender, EventArgs e)
    {

    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
