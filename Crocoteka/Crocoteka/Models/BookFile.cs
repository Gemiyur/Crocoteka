using System.IO;
using LiteDB;

namespace Crocoteka.Models;

/// <summary>
/// Класс файла книги.
/// </summary>
public class BookFile : BaseModel
{
    private string filename = string.Empty;

    /// <summary>
    /// Имя файла книги с полным путём.
    /// </summary>
    public string Filename
    {
        get => filename;
        set
        {
            filename = value ?? string.Empty;
            OnPropertyChanged("Filename");
            OnPropertyChanged("Folder");
            OnPropertyChanged("Name");
            OnPropertyChanged("NameOnly");
            OnPropertyChanged("Extension");
            OnPropertyChanged("IsAudio");
            OnPropertyChanged("IsText");
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

    private string comment = string.Empty;

    /// <summary>
    /// Комментарий к книге.
    /// </summary>
    public string Comment
    {
        get => comment;
        set
        {
            comment = value ?? string.Empty;
            OnPropertyChanged("Comment");
        }
    }

    /// <summary>
    /// Возвращает папку файла книги.
    /// </summary>
    [BsonIgnore]
    public string Folder => Path.GetDirectoryName(filename) ?? string.Empty;

    /// <summary>
    /// Возвращает имя файла книги с расширением.
    /// </summary>
    [BsonIgnore]
    public string Name => Path.GetFileName(filename);

    /// <summary>
    /// Возвращает имя файла книги без расширения.
    /// </summary>
    [BsonIgnore]
    public string NameOnly => Path.GetFileNameWithoutExtension(filename);

    /// <summary>
    /// Возвращает расширение файла книги.
    /// </summary>
    [BsonIgnore]
    public string Extension => Path.GetExtension(filename);

    /// <summary>
    /// Возвращает является ли файл аудио книгой.
    /// </summary>
    [BsonIgnore]
    public bool IsAudio => App.AudioExtensions.Exists(x => x.Equals(Extension, StringComparison.CurrentCultureIgnoreCase));

    /// <summary>
    /// Возвращает является ли файл текстовой книгой.
    /// </summary>
    [BsonIgnore]
    public bool IsText => App.TextExtensions.Exists(x => x.Equals(Extension, StringComparison.CurrentCultureIgnoreCase));

    /// <summary>
    /// Существует ли файл книги.
    /// </summary>
    [BsonIgnore]
    public bool Exists => File.Exists(filename);
}
