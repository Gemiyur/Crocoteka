namespace Crocoteka.Models;

/// <summary>
/// Класс раздела текстовой книги.
/// </summary>
public class TextChapter : BaseModel
{
    private int position;

    /// <summary>
    /// Позиция раздела в файле книги.
    /// </summary>
    public int Position
    {
        get => position;
        set
        {
            position = value;
            OnPropertyChanged("Position");
        }
    }

    private string title = string.Empty;

    /// <summary>
    /// Название раздела книги.
    /// </summary>
    public string Title
    {
        get => title;
        set
        {
            title = value;
            OnPropertyChanged("Title");
        }
    }

    /// <summary>
    /// Список подразделов раздела книги.
    /// </summary>
    public List<TextChapter> Chapters { get; set; } = [];
}
