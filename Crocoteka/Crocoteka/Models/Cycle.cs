using Gemiyur.Comparers;

namespace Crocoteka.Models;

/// <summary>
/// Класс цикла книг.
/// </summary>
public class Cycle : BaseModel
{
    /// <summary>
    /// Идентификатор цикла книг.
    /// </summary>
    public int CycleId { get; set; }

    private string title = string.Empty;

    /// <summary>
    /// Название цикла книг.
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

    // TODO: Модель данных изменена и этот компаратор для сортировки книг цикла не годится.

    ///// <summary>
    ///// Компаратор сравнения книг для сортировки книг в цикле книг.
    ///// </summary>
    ///// <remarks>
    ///// Данный компаратор выбран для сортировки по алфавиту внутри номера книги в цикле,
    ///// если есть одинаковые номера книг, в том числе если номеров книг нет или они не указаны (номер = 0).
    ///// </remarks>
    //public static readonly MultiKeyComparer BooksComparer =
    //    new([new IntKeyComparer(x => ((Book)x).Number), new StringKeyComparer(x => ((Book)x).Title)]);
}
