using LiteDB;

namespace Crocoteka.Models;

/// <summary>
/// Класс закладки аудиокниги.
/// </summary>
public class AudioBookmark : BaseModel
{
    private TimeSpan position;

    /// <summary>
    /// Позиция закладки аудиокниги.
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

    /// <summary>
    /// Позиция закладки аудиокниги в виде строки.
    /// </summary>
    [BsonIgnore]
    public string PositionText => App.TimeSpanToString(Position);

    private string title = string.Empty;

    /// <summary>
    /// Название закладки аудиокниги.
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
