using System.Windows;
using Crocoteka.Models;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс окна "О серии".
/// </summary>
public partial class CycleInfoDialog : Window
{
    public CycleInfoDialog(Cycle cycle)
    {
        InitializeComponent();
        TitleTextBlock.FontSize = FontSize + 2;
        TitleTextBlock.Text = cycle.Title;
        AnnotationTextBox.Text = cycle.Annotation;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (Properties.Settings.Default.SaveInfoWindowsLocation &&
            App.SizeDefined(Properties.Settings.Default.CycleInfoWindowSize))
        {
            Left = Properties.Settings.Default.CycleInfoWindowPos.X;
            Top = Properties.Settings.Default.CycleInfoWindowPos.Y;
            Width = Properties.Settings.Default.CycleInfoWindowSize.Width;
            Height = Properties.Settings.Default.CycleInfoWindowSize.Height;
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (Properties.Settings.Default.SaveInfoWindowsLocation)
        {
            Properties.Settings.Default.CycleInfoWindowPos = new System.Drawing.Point((int)Left, (int)Top);
            Properties.Settings.Default.CycleInfoWindowSize = new System.Drawing.Size((int)Width, (int)Height);
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
