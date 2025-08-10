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

    /// <summary>
    /// Список авторов книги.
    /// </summary>
    [BsonRef("Authors")]
    public List<Author> Authors { get; set; } = [];

    /// <summary>
    /// Возвращает список авторов книги в виде строки Имя-Фамилия.
    /// Список отсортирован по Фамилия-Имя.
    /// </summary>
    [BsonIgnore]
    public string AuthorNamesFirstLast =>
        App.ListToString(Authors.OrderBy(x => x.NameLastFirst), ", ", x => ((Author)x).NameFirstLast);

    /// <summary>
    /// Возвращает список авторов книги в виде строки Имя-Отчество-Фамилия.
    /// Список отсортирован по Фамилия-Имя-Отчество.
    /// </summary>
    [BsonIgnore]
    public string AuthorNamesFirstMiddleLast =>
        App.ListToString(Authors.OrderBy(x => x.NameLastFirstMiddle), ", ", x => ((Author)x).NameFirstMiddleLast);

    /// <summary>
    /// Возвращает список авторов книги в виде строки Фамилия-Имя.
    /// Список отсортирован по Фамилия-Имя.
    /// </summary>
    [BsonIgnore]
    public string AuthorNamesLastFirst =>
        App.ListToString(Authors.OrderBy(x => x.NameLastFirst), ", ", x => ((Author)x).NameLastFirst);

    /// <summary>
    /// Возвращает список авторов книги в виде строки Фамилия-Имя-Отчество.
    /// Список отсортирован по Фамилия-Имя-Отчество.
    /// </summary>
    [BsonIgnore]
    public string AuthorNamesLastFirstMiddle =>
        App.ListToString(Authors.OrderBy(x => x.NameLastFirstMiddle), ", ", x => ((Author)x).NameLastFirstMiddle);

    private string annotation = string.Empty;

    /// <summary>
    /// Аннотация к книге.
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

    private int cycleNumber;

    /// <summary>
    /// Номер книги в серии книг.
    /// </summary>
    public int CycleNumber
    {
        get => cycleNumber;
        set
        {
            cycleNumber = value;
            OnPropertyChanged("CycleNumber");
            OnPropertyChanged("CyclePart");
            OnPropertyChanged("CyclePartText");
        }
    }

    /// <summary>
    /// Возвращает номер книги в серии книг в виде строки.
    /// </summary>
    /// <remarks>Для нуля возвращает пустую строку.</remarks>
    [BsonIgnore]
    public string CyclePart => CycleNumber > 0 ? CycleNumber.ToString() : "";

    /// <summary>
    /// Возвращает строку номера книги серии для отображения.
    /// </summary>
    [BsonIgnore]
    public string CyclePartText => CycleNumber > 0 ? $"Номер в серии: {CyclePart}" : "Номер в серии не указан";

    /// <summary>
    /// Список жанров книги.
    /// </summary>
    [BsonRef("Genres")]
    public List<Genre> Genres { get; set; } = [];

    /// <summary>
    /// Список файлов книги.
    /// </summary>
    public List<BookFile> Files { get; set; } = [];

    /// <summary>
    /// Возвращает количество аудио файлов книги.
    /// </summary>
    [BsonIgnore]
    public int AudioCount => Files.Count > 0 ? Files.Count(x => x.IsAudio) : 0;

    /// <summary>
    /// Возвращает строку количества аудио файлов книги для отображения.
    /// </summary>
    [BsonIgnore]
    public string AudioCountText => $"Аудио: {AudioCount}";

    /// <summary>
    /// Возвращает количество текстовых файлов книги.
    /// </summary>
    [BsonIgnore]
    public int TextCount => Files.Count > 0 ? Files.Count(x => x.IsText) : 0;

    /// <summary>
    /// Возвращает строку количества текстовых файлов книги для отображения.
    /// </summary>
    [BsonIgnore]
    public string TextCountText => $"Текст: {TextCount}";

    /// <summary>
    /// Возвращает есть ли аудио файлы книги.
    /// </summary>
    [BsonIgnore]
    public bool HasAudio => AudioCount > 0;

    /// <summary>
    /// Возвращает есть ли текстовые файлы книги.
    /// </summary>
    [BsonIgnore]
    public bool HasText => TextCount > 0;
}
