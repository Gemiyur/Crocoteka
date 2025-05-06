using LiteDB;
using System.Windows.Media.Imaging;

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
        }
    }

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
    /// Изображение обложки раздела аудиокниги.
    /// </summary>
    [BsonIgnore]
    public BitmapFrame? Picture => PictureData != null ? App.GetBitmap(PictureData) : null;

    /// <summary>
    /// Массив байт изображения обложки раздела аудиокниги.
    /// </summary>
    public byte[]? PictureData;

    /// <summary>
    /// Список подразделов раздела аудиокниги.
    /// </summary>
    public List<AudioChapter> Chapters { get; set; } = [];
}
