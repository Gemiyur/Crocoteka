using LiteDB;

namespace Crocoteka.Models;

/// <summary>
/// Класс файла аудиокниги.
/// </summary>
public class AudioFile : BookFile
{
    /// <summary>
    /// Продолжительность аудиокниги.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Продолжительность аудиокниги в виде строки.
    /// </summary>
    [BsonIgnore]
    public string DurationText => App.TimeSpanToString(Duration);

    private string lector = string.Empty;

    /// <summary>
    /// Чтец аудиокниги.
    /// </summary>
    public string Lector
    {
        get => lector;
        set
        {
            lector = value ?? string.Empty;
            OnPropertyChanged("Lector");
        }
    }

    /// <summary>
    /// Список разделов аудиокниги.
    /// </summary>
    public List<AudioChapter> Chapters { get; set; } = [];

    /// <summary>
    /// Позиция воспроизведения.
    /// </summary>
    public TimeSpan PlayPosition { get; set; }

    /// <summary>
    /// Позиция воспроизведения в виде строки.
    /// </summary>
    [BsonIgnore]
    public string PlayPositionText => App.TimeSpanToString(PlayPosition);

    /// <summary>
    /// Находится ли аудиокниги в состоянии прослушивания.
    /// </summary>
    [BsonIgnore]
    public bool Listening => PlayPosition > TimeSpan.Zero;

    /// <summary>
    /// Список закладок аудиокниги.
    /// </summary>
    public List<AudioBookmark> Bookmarks { get; set; } = [];
}
