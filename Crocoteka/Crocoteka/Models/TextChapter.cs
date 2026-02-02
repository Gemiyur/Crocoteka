namespace Crocoteka.Models;

/// <summary>
/// Класс раздела текстовой книги.
/// </summary>
public class TextChapter : BaseModel
{
    private int position;

    /// <summary>
    /// Позиция раздела текстовой книги.
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
    /// Название раздела текстовой книги.
    /// </summary>
    public string Title
    {
        get => title;
        set
        {
            title = value ?? string.Empty;
            OnPropertyChanged("Title");
        }
    }
}
