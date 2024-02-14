using System;
using System.Collections.Generic;

namespace BookCatalog.API.Model;

public partial class BookGenre
{
    public long Id { get; set; }

    public long BookId { get; set; }

    public long? GenreId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Genre? Genre { get; set; }
}
