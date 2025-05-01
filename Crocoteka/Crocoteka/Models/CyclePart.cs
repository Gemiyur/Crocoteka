using LiteDB;

namespace Crocoteka.Models;

public class CyclePart : BaseModel
{
    [BsonRef("Cycles")]
    public Cycle? Cycle { get; set; }

    public int Number { get; set; }
}
