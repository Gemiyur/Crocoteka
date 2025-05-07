using System.Windows.Media.Imaging;
using ATL;

namespace Crocoteka.Media;

/// <summary>
/// Класс данных аудиокниги из тега.
/// </summary>
public class TrackData
{
    /// <summary>
    /// Название книги.
    /// </summary>
    public string Title;

    /// <summary>
    /// Автор книги.
    /// </summary>
    public string Author;

    /// <summary>
    /// Комментарий к книге.
    /// </summary>
    public string Comment;

    /// <summary>
    /// Описание книги.
    /// </summary>
    public string Description;

    /// <summary>
    /// Дополнительное описание книги.
    /// </summary>
    public string Lyrics;

    /// <summary>
    /// Оглавление (список разделов) книги.
    /// </summary>
    public List<ChapterData> Chapters = [];

    /// <summary>
    /// Продолжительность воспроизведения книги в секундах.
    /// </summary>
    public TimeSpan Duration;

    /// <summary>
    /// Жанры книги.
    /// </summary>
    public string Genre;

    /// <summary>
    /// Список изображений обложек книги.
    /// </summary>
    public List<BitmapFrame> Pictures = [];

    /// <summary>
    /// Список массивов байт обложек книги.
    /// </summary>
    public List<byte[]> PicturesData = [];

    /// <summary>
    /// Название цикла книг.
    /// </summary>
    public string SeriesTitle;

    /// <summary>
    /// Номер книги в цикле книг.
    /// </summary>
    public string SeriesPart;

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
    /// <param name="filename">Имя файла книги с полным путём.</param>
    public TrackData(string filename)
    {
        var track = new Track(filename);
        Title = track.Title;
        Author = track.Artist;
        Comment = track.Comment;
        Description = track.Description;
        Lyrics = track.Lyrics.UnsynchronizedLyrics;
        foreach (var chapter in track.Chapters)
        {
            var chapterData = new ChapterData()
            {
                Title = chapter.Title,
                StartTime = TimeSpan.FromMilliseconds(chapter.StartTime),
                EndTime = TimeSpan.FromMilliseconds(chapter.EndTime),
            };
            Chapters.Add(chapterData);
        }
        Duration = TimeSpan.FromSeconds(track.Duration);
        Genre = track.Genre;
        foreach (var picture in track.EmbeddedPictures)
        {
            Pictures.Add(App.GetBitmap(picture.PictureData));
            PicturesData.Add(picture.PictureData);
        }
        SeriesTitle = track.SeriesTitle;
        SeriesPart = track.SeriesPart;
    }
}
