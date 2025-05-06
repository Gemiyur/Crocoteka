using LiteDB;

namespace Crocoteka.Models;

/// <summary>
/// Класс раздела аудиокниги.
/// </summary>
public class AudioChapter : BaseModel
{
    private TimeSpan startTime;

    /// <summary>
    /// Позиция начала раздела в файле аудиокниги.
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
        }
    }

    /// <summary>
    /// Позиция начала раздела в файле аудиокниги в виде строки.
    /// </summary>
    [BsonIgnore]
    public string StartTimeText => App.TimeSpanToString(StartTime);

    private TimeSpan endTime;

    /// <summary>
    /// Позиция конца раздела в файле аудиокниги.
    /// </summary>
    public TimeSpan EndTime
    {
        get => endTime;
        set
        {
            endTime = value;
            OnPropertyChanged("EndTime");
            OnPropertyChanged("Duration");
        }
    }

    /// <summary>
    /// Продолжительность раздела аудиокниги.
    /// </summary>
    [BsonIgnore]
    public TimeSpan Duration => EndTime - StartTime;

    private string title = string.Empty;

    /// <summary>
    /// Название раздела аудиокниги.
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
    /// Список подразделов раздела аудиокниги.
    /// </summary>
    public List<AudioChapter> Chapters { get; set; } = [];
}
