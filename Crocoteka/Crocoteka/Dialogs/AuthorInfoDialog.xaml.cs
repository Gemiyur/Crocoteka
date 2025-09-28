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

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
