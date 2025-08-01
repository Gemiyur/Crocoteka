using System.IO;
using System.Windows.Media.Imaging;
using LiteDB;

namespace Crocoteka.Models;

/// <summary>
/// Класс книги.
/// </summary>
public class Book : BaseModel
{
    /// <summary>
    /// Идентификатор книги.
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Это аудиокнига?
    /// </summary>
    [BsonIgnore]
    public bool IsAudio => FileExtension == ".m4b";

    private string title = string.Empty;

    /// <summary>
    /// Название книги.
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

    private string annotation = string.Empty;

    /// <summary>
    /// Аннотация книги.
    /// </summary>
    public string Annotation
    {
        get => annotation;
        set
        {
            annotation = value ?? string.Empty;
            OnPropertyChanged("Annotation");
        }
    }

    /// <summary>
    /// Список авторов книги.
    /// </summary>
    [BsonRef("Authors")]
    public List<Author> Authors { get; set; } = [];

    /// <summary>
    /// Изображение обложки книги.
    /// </summary>
    [BsonIgnore]
    public BitmapFrame? Cover => CoverData != null ? App.GetBitmap(CoverData) : null;

    /// <summary>
    /// Массив байт изображения обложки книги.
    /// </summary>
    public byte[]? CoverData { get; set; }

    private Cycle? cycle;

    /// <summary>
    /// Серия книг.
    /// </summary>
    [BsonRef("Cycles")]
    public Cycle? Cycle
    {
        get => cycle;
        set
        {
            cycle = value;
            OnPropertyChanged("Cycle");
            OnPropertyChanged("CycleTitle");
        }
    }

    /// <summary>
    /// Возвращает название серии книг.
    /// </summary>
    [BsonIgnore]
    public string CycleTitle => Cycle != null ? Cycle.Title : string.Empty;

    private string lector = string.Empty;

    /// <summary>
    /// Чтец книги.
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

    private string translator = string.Empty;

    /// <summary>
    /// Переводчик книги.
    /// </summary>
    public string Translator
    {
        get => translator;
        set
        {
            translator = value ?? string.Empty;
            OnPropertyChanged("Translator");
        }
    }

    /// <summary>
    /// Список жанров книги.
    /// </summary>
    [BsonRef("Genres")]
    public List<Genre> Genres { get; set; } = [];

    /// <summary>
    /// Файл книги с полным путём.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Существует ли файл книги.
    /// </summary>
    [BsonIgnore]
    public bool FileExists => File.Exists(FileName);

    /// <summary>
    /// Возвращает расширение файла книги. Начинается с точки.
    /// </summary>
    [BsonIgnore]
    public string FileExtension => Path.GetExtension(FileName);

    /// <summary>
    /// Размер файла книги в байтах.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Продолжительность аудио книги.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Продолжительность аудио книги в виде строки.
    /// </summary>
    [BsonIgnore]
    public string DurationText => App.TimeSpanToString(Duration);

    /// <summary>
    /// Позиция воспроизведения аудиокниги.
    /// </summary>
    public TimeSpan PlayPosition { get; set; }

    /// <summary>
    /// Позиция воспроизведения аудиокниги в виде строки.
    /// </summary>
    [BsonIgnore]
    public string PlayPositionText => App.TimeSpanToString(PlayPosition);

    /// <summary>
    /// Находится ли аудиокнига в состоянии прослушивания.
    /// </summary>
    [BsonIgnore]
    public bool IsListening => PlayPosition > TimeSpan.Zero;

    /// <summary>
    /// Позиция чтения текстовой книги.
    /// </summary>
    public int ReadPosition { get; set; }

    /// <summary>
    /// Находится ли текстовая книга в состоянии чтения.
    /// </summary>
    [BsonIgnore]
    public bool IsReading => ReadPosition > 0;
}
