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
        if (Properties.Settings.Default.SaveInfoWindowsLocation &&
            App.SizeDefined(Properties.Settings.Default.AuthorInfoWindowSize))
        {
            Left = Properties.Settings.Default.AuthorInfoWindowPos.X;
            Top = Properties.Settings.Default.AuthorInfoWindowPos.Y;
            Width = Properties.Settings.Default.AuthorInfoWindowSize.Width;
            Height = Properties.Settings.Default.AuthorInfoWindowSize.Height;
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (Properties.Settings.Default.SaveInfoWindowsLocation)
        {
            Properties.Settings.Default.AuthorInfoWindowPos = new System.Drawing.Point((int)Left, (int)Top);
            Properties.Settings.Default.AuthorInfoWindowSize = new System.Drawing.Size((int)Width, (int)Height);
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
