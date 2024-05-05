using System.Text.Json.Serialization;

namespace BookCatalog.API.Model;

public partial class BookPublisher
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
