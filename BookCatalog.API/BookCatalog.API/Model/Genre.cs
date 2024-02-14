using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookCatalog.API.Model;

public partial class Genre
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}
