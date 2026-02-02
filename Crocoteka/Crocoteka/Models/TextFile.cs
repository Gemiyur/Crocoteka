using LiteDB;

namespace Crocoteka.Models;

/// <summary>
/// Класс файла текстовой книги.
/// </summary>
public class TextFile : BookFile
{
    /// <summary>
    /// Список разделов текстовой книги.
    /// </summary>
    public List<TextChapter> Chapters { get; set; } = [];

    /// <summary>
    /// Позиция чтения.
    /// </summary>
    public int ReadPosition { get; set; }

    /// <summary>
    /// Находится ли текстовая книга в состоянии чтения.
    /// </summary>
    [BsonIgnore]
    public bool Reading => ReadPosition > 0;

    /// <summary>
    /// Список закладок текстовой книги.
    /// </summary>
    public List<TextBookmark> Bookmarks { get; set; } = [];
}
