namespace Crocoteka.Models;

/// <summary>
/// Класс закладки аудиокниги.
/// </summary>
public class AudioBookmark : BaseModel
{
    private TimeSpan position;

    /// <summary>
    /// Позиция закладки в файле аудиокниги.
    /// </summary>
    public TimeSpan Position
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
    /// Название закладки аудиокниги.
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
