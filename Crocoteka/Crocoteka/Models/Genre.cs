namespace Crocoteka.Models;

/// <summary>
/// Класс жанра.
/// </summary>
public class Genre : BaseModel
{
    /// <summary>
    /// Идентификатор жанра.
    /// </summary>
    public int GenreId { get; set; }

    private string title = string.Empty;

    /// <summary>
    /// Название жанра.
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
    /// Копирует данные жанра в указанный жанр.
    /// </summary>
    /// <param name="genre">Жанр, в который копируются данные жанра.</param>
    /// <remarks>
    /// Копируются все данные жанра кроме идентификатора.
    /// </remarks>
    public void CopyTo(Genre genre)
    {
        genre.Title = Title;
    }

    /// <summary>
    /// Создаёт и возвращает неполную копию жанра.
    /// </summary>
    /// <returns>Неполная копия жанра.</returns>
    /// <remarks>
    /// Копия содержит все данные жанра кроме идентификатора.
    /// </remarks>
    public Genre Clone()
    {
        var genre = new Genre();
        CopyTo(genre);
        return genre;
    }
}
