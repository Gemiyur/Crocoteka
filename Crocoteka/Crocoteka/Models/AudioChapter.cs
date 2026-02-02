using LiteDB;

namespace Crocoteka.Models;

/// <summary>
/// Класс раздела аудиокниги.
/// </summary>
public class AudioChapter : BaseModel
{
    private TimeSpan startTime;

    /// <summary>
    /// Позиция начала раздела аудиокниги.
    /// </summary>
    public TimeSpan StartTime
    {
        get => startTime;
        set
        {
            startTime = value;
            OnPropertyChanged("StartTime");
            OnPropertyChanged("StartTimeText");
            OnPropertyChanged("Duration");
            OnPropertyChanged("DurationText");
        }
    }

    /// <summary>
    /// Позиция начала раздела аудиокниги в виде строки.
    /// </summary>
    [BsonIgnore]
    public string StartTimeText => App.TimeSpanToString(StartTime);

    private TimeSpan endTime;

    /// <summary>
    /// Позиция конца раздела аудиокниги.
    /// </summary>
    public TimeSpan EndTime
    {
        get => endTime;
        set
        {
            endTime = value;
            OnPropertyChanged("EndTime");
            OnPropertyChanged("Duration");
            OnPropertyChanged("DurationText");
        }
    }

    /// <summary>
    /// Продолжительность раздела аудиокниги.
    /// </summary>
    [BsonIgnore]
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    /// Продолжительность аудиокниги книги в виде строки.
    /// </summary>
    [BsonIgnore]
    public string DurationText => App.TimeSpanToString(Duration);

    private string title = string.Empty;

    /// <summary>
    /// Название раздела аудиокниги.
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
