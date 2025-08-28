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

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс редактора авторов.
/// </summary>
public partial class AuthorsEditor : Window
{
    public AuthorsEditor()
    {
        InitializeComponent();
    }

    private void AuthorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var editor = new AuthorEditor() { Owner = this };
        editor.ShowDialog();
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {

    }
}
