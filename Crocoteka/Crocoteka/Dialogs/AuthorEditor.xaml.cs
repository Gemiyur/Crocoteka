using System.Windows;
using System.Windows.Controls;
using Crocoteka.Models;
using Crocoteka.Tools;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс редактора автора.
/// </summary>
public partial class AuthorEditor : Window
{
    /// <summary>
    /// Возвращает было ли изменено имя, фамилия или отчество автора.
    /// </summary>
    public bool NameChanged { get; private set; }

    /// <summary>
    /// Редактируемый автор.
    /// </summary>
    private readonly Author author;

    /// <summary>
    /// Фамилия автора при загрузке в редактор.
    /// </summary>
    private readonly string origLastName;

    /// <summary>
    /// Имя автора при загрузке в редактор.
    /// </summary>
    private readonly string origFirstName;

    /// <summary>
    /// Отчество автора при загрузке в редактор.
    /// </summary>
    private readonly string origMiddleName;

    /// <summary>
    /// Об авторе при загрузке в редактор.
    /// </summary>
    private readonly string origAbout;

    /// <summary>
    /// Была ли ошибка сохранения.
    /// </summary>
    private bool wasSaveError;

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
    /// <param name="author">Автор.</param>
    public AuthorEditor(Author author)
    {
        InitializeComponent();

        this.author = author;

        origLastName = author.LastName;
        origFirstName = author.FirstName;
        origMiddleName = author.MiddleName;
        origAbout = author.About;

        LastNameTextBox.Text = author.LastName;
        FirstNameTextBox.Text = author.FirstName;
        MiddleNameTextBox.Text = author.MiddleName;
        AboutTextBox.Text = author.About;
    }

    /// <summary>
    /// Проверяет доступность кнопки Сохранить.
    /// </summary>
    private void CheckSaveButton()
    {
        var nameEmpty =
            string.IsNullOrWhiteSpace(LastNameTextBox.Text) &&
            string.IsNullOrWhiteSpace(FirstNameTextBox.Text) &&
            string.IsNullOrWhiteSpace(MiddleNameTextBox.Text);

        NameChanged =
            LastNameTextBox.Text.Trim() != author.LastName ||
            FirstNameTextBox.Text.Trim() != author.FirstName ||
            MiddleNameTextBox.Text.Trim() != author.MiddleName;

        SaveButton.IsEnabled = !nameEmpty && (NameChanged || AboutTextBox.Text.Trim() != author.About);
    }

    /// <summary>
    /// Восстанавливает свойства автора, которые были до редактирования.
    /// </summary>
    private void RestoreOriginal()
    {
        author.LastName = origLastName;
        author.FirstName = origFirstName;
        author.MiddleName = origMiddleName;
        author.About = origAbout;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (wasSaveError)
            RestoreOriginal();
    }

    private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e) => CheckSaveButton();

    private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e) => CheckSaveButton();

    private void MiddleNameTextBox_TextChanged(object sender, TextChangedEventArgs e) => CheckSaveButton();

    private void AboutTextBox_TextChanged(object sender, TextChangedEventArgs e) => CheckSaveButton();

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var lastName = LastNameTextBox.Text.Trim();
        var firstName = FirstNameTextBox.Text.Trim();
        var middleName = MiddleNameTextBox.Text.Trim();
        var about = AboutTextBox.Text.Trim();

        var fio = Author.ConcatNames(lastName, firstName, middleName);

        if (Library.Authors.Exists(x => x.NameLastFirstMiddle.Equals(fio, StringComparison.CurrentCultureIgnoreCase)))
        {
            if ((author.AuthorId > 0 && NameChanged) || author.AuthorId < 1)
            {
                MessageBox.Show("Автор с таким именем уже существует.", Title);
                return;
            }
        }

        author.LastName = lastName;
        author.FirstName = firstName;
        author.MiddleName = middleName;
        author.About = about;

        wasSaveError = author.AuthorId > 0 ? !Library.UpdateAuthor(author) : !Library.AddAuthor(author);
        if (wasSaveError)
        {
            MessageBox.Show("Не удалось сохранить автора.", Title);
            return;
        }

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
