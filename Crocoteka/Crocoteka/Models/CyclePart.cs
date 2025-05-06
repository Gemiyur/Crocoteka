using LiteDB;

namespace Crocoteka.Models;

/// <summary>
/// Класс части (номера книги) в цикле книг.
/// </summary>
public class CyclePart : BaseModel
{
    private Cycle? cycle;

    /// <summary>
    /// Цикл книг.
    /// </summary>
    [BsonRef("Cycles")]
    public Cycle? Cycle
    {
        get => cycle;
        set
        {
            cycle = value;
            OnPropertyChanged("Cycle");
        }
    }

    private int number;

    /// <summary>
    /// Номер книги в цикле книг.
    /// </summary>
    public int Number
    {
        get => number;
        set
        {
            number = value;
            OnPropertyChanged("Number");
        }
    }
}
