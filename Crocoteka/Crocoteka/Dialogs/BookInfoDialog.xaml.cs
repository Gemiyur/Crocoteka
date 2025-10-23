using Crocoteka.Models;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс окна "О книге".
/// </summary>
public partial class BookInfoDialog : Window
{
    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    private const int GWL_STYLE = -16;
    private const int WS_MAXIMIZEBOX = 0x10000;
    private const int WS_MINIMIZEBOX = 0x20000;

    private readonly Book book;

    public BookInfoDialog(Book book)
    {
        InitializeComponent();
        this.book = book;
        InitializeAuthors();
        InitializeTitle();
        InitializeCycle();
        InitializeAnnotation();
        InitializeGenres();
        InitializeFiles();
    }

    private void InitializeAuthors()
    {
        for (int i = 0; i < book.Authors.Count; i++)
        {
            var run = Properties.Settings.Default.BookInfoAuthorFullName
                ? new Run(book.Authors[i].NameFirstMiddleLast)
                : new Run(book.Authors[i].NameFirstLast);
            var link = new Hyperlink(run);
            link.Tag = book.Authors[i];
            link.Style = (Style)FindResource("HyperlinkStyle");
            link.Click += AuthorLink_Click;
            AuthorsTextBlock.Inlines.Add(link);
            if (i < book.Authors.Count - 1)
                AuthorsTextBlock.Inlines.Add(new Run(", "));
        }
    }

    private void InitializeTitle()
    {
        TitleTextBlock.FontSize = FontSize + 2;
        TitleTextBlock.Text = book.Title;
    }

    private void InitializeCycle()
    {
        if (book.Cycle != null)
        {
            CycleGrid.Visibility = Visibility.Visible;
            var link = new Hyperlink(new Run(book.Cycle.Title));
            link.Tag = book.Cycle;
            link.Style = (Style)FindResource("HyperlinkStyle");
            link.Click += CycleLink_Click;
            CycleTitleTextBlock.Inlines.Add(link);
            if (book.CycleNumbers.Length > 0)
            {
                CycleNumbersStackPanel.Visibility = Visibility.Visible;
                CycleNumbersTextBlock.Text = book.CycleNumbers;
            }
            else
                CycleNumbersStackPanel.Visibility = Visibility.Collapsed;
        }
        else
        {
            CycleGrid.Visibility = Visibility.Collapsed;
        }
    }

    private void InitializeAnnotation()
    {
        AnnotationTextBox.Text = book.Annotation;
    }

    private void InitializeGenres()
    {
        if (book.Genres.Count > 0)
        {
            GenresGrid.Visibility = Visibility.Visible;
            GenresTextBlock.Text =
                App.ListToString(book.Genres, "; ", x => ((Genre)x).Title, StringComparer.CurrentCultureIgnoreCase);
        }
        else
        {
            GenresGrid.Visibility = Visibility.Collapsed;
        }
    }

    private void InitializeFiles()
    {
        TotalFilesTextBlock.Text += book.FilesCountText;
        AudioFilesTextBlock.Text += book.AudioCountText;
        TextFilesTextBlock.Text += book.TextCountText;
        if (book.UnknownCount > 0)
            UnknownFilesTextBlock.Text += book.UnknownCountText;
        else
            UnknownFilesTextBlock.Visibility = Visibility.Collapsed;
        if (book.NotFoundCount > 0)
            NotFoundFilesTextBlock.Text += book.NotFoundCountText;
        else
            NotFoundFilesTextBlock.Visibility = Visibility.Collapsed;
        List<BookFile> files = [];
        files.AddRange(book.Files.OrderBy(x => x.Filename, StringComparer.CurrentCultureIgnoreCase));
        FilesListBox.ItemsSource = files;
    }

    private void Window_SourceInitialized(object sender, EventArgs e)
    {
        IntPtr handle = new WindowInteropHelper(this).Handle;
        _ = SetWindowLong(handle, GWL_STYLE, GetWindowLong(handle, GWL_STYLE) & ~WS_MINIMIZEBOX);
        _ = SetWindowLong(handle, GWL_STYLE, GetWindowLong(handle, GWL_STYLE) & ~WS_MAXIMIZEBOX);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (Properties.Settings.Default.SaveInfoWindowsLocation &&
            App.SizeDefined(Properties.Settings.Default.BookInfoWindowSize))
        {
            Left = Properties.Settings.Default.BookInfoWindowPos.X;
            Top = Properties.Settings.Default.BookInfoWindowPos.Y;
            Width = Properties.Settings.Default.BookInfoWindowSize.Width;
            Height = Properties.Settings.Default.BookInfoWindowSize.Height;
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (Properties.Settings.Default.SaveInfoWindowsLocation)
        {
            Properties.Settings.Default.BookInfoWindowPos = new System.Drawing.Point((int)Left, (int)Top);
            Properties.Settings.Default.BookInfoWindowSize = new System.Drawing.Size((int)Width, (int)Height);
        }
    }

    private void AuthorLink_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Hyperlink)
            return;
        var author = (Author)((Hyperlink)sender).Tag;
        new AuthorInfoDialog(author) { Owner = this }.ShowDialog();
    }

    private void CycleLink_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Hyperlink)
            return;
        var cycle = (Cycle)((Hyperlink)sender).Tag;
        new CycleInfoDialog(cycle) { Owner = this }.ShowDialog();
    }

    private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FilesListBox.SelectedIndex >= 0)
            FilesListBox.SelectedIndex = -1;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
