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
/// Класс окна поиска файлов книг.
/// </summary>
public partial class FindFilesWindow : Window
{
    public FindFilesWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //if (Properties.Settings.Default.SaveBookWindowsLocation &&
        //    App.SizeDefined(Properties.Settings.Default.BookInfoSize))
        //{
        //    Left = Properties.Settings.Default.BookInfoPos.X;
        //    Top = Properties.Settings.Default.BookInfoPos.Y;
        //    Width = Properties.Settings.Default.BookInfoSize.Width;
        //    Height = Properties.Settings.Default.BookInfoSize.Height;
        //}
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        //if (Properties.Settings.Default.SaveBookWindowsLocation)
        //{
        //    Properties.Settings.Default.BookInfoPos = new System.Drawing.Point((int)Left, (int)Top);
        //    Properties.Settings.Default.BookInfoSize = new System.Drawing.Size((int)Width, (int)Height);
        //}
        //App.GetMainWindow().Activate();
    }

    private void FolderButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void NotInLibraryCheckBox_Click(object sender, RoutedEventArgs e)
    {

    }

    private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void BookButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
