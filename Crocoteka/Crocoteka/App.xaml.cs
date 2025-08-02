using System.Windows;

namespace Crocoteka;

/// <summary>
/// Класс приложения.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Имя файла базы данных с полным путём.
    /// </summary>
    public static string? DbName { get; set; }

    /// <summary>
    /// Расширения файлов аудио книг.
    /// </summary>
    public static readonly List<string> AudioExtensions = [".m4b", ".mp3"];

    /// <summary>
    /// Расширения файлов текстовых книг.
    /// </summary>
    public static readonly List<string> TextExtensions = [".fb2", ".epub"];
}
