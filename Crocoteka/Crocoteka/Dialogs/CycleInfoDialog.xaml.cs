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

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
