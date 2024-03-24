using System;
using System.Collections.Generic;

namespace BookCatalog.API.Model;

public partial class BookPublisher
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
