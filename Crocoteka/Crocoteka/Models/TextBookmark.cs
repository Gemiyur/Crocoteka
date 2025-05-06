namespace Crocoteka.Models;

/// <summary>
/// Класс закладки текстовой книги.
/// </summary>
public class TextBookmark : BaseModel
{
    private int position;

    /// <summary>
    /// Позиция закладки в файле текстовой книги.
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
    /// Название закладки текстовой книги.
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
}
