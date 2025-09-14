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
            //OnPropertyChanged("Exists");
            OnPropertyChanged("IsAudio");
            OnPropertyChanged("IsText");
            OnPropertyChanged("IsZip");
            OnPropertyChanged("TypeText");
        }
    }

    private string comment = string.Empty;

    /// <summary>
    /// Комментарий к файлу книги.
    /// </summary>
    public string Comment
    {
        get => comment;
        set
        {
            comment = value ?? string.Empty;
            OnPropertyChanged("Comment");
            OnPropertyChanged("ListItemComment");
        }
    }

    /// <summary>
    /// Возвращает текст комментария для отображения в списке файлов.
    /// </summary>
    [BsonIgnore]
    public string ListItemComment => string.IsNullOrWhiteSpace(Comment) ? TypeText : $"{TypeText}. {Comment}";

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
    /// Возвращает существует ли файл книги.
    /// </summary>
    [BsonIgnore]
    public bool Exists => File.Exists(filename);

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
    /// Возвращает тест типа файла книги (аудио, текст, архив).
    /// </summary>
    [BsonIgnore]
    public string TypeText
    {
        get
        {
            if (IsAudio)
                return "Аудио";
            else if (IsText)
                return "Текст";
            else
                return "Неизвестный";
        }
    }
}
