using System;
using System.Collections.Generic;
using BookCatalog.API.Model;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.API.Infrastructure;

public partial class BookContext : DbContext
{
    public BookContext()
    {
    }
    public BookContext(DbContextOptions<BookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookFormat> BookFormats { get; set; }

    public virtual DbSet<BookGenre> BookGenres { get; set; }

    public virtual DbSet<BookPublisher> BookPublishers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn" })
            .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
            .HasPostgresEnum("pgsodium", "key_type", new[] { "aead-ietf", "aead-det", "hmacsha512", "hmacsha256", "auth", "shorthash", "generichash", "kdf", "secretbox", "secretstream", "stream_xchacha20" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "pgjwt")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("pgroonga")
            .HasPostgresExtension("pgsodium", "pgsodium")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("books_pkey");

            entity.ToTable("books");

            entity.HasIndex(e => e.Url, "books_url_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorName).HasColumnName("author_name");
            entity.Property(e => e.Availability)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("availability");
            entity.Property(e => e.AverageRating)
                .HasDefaultValueSql("'0'::real")
                .HasColumnName("average_rating");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("''::text")
                .HasColumnName("description");
            entity.Property(e => e.Dimensions)
                .HasDefaultValueSql("''::text")
                .HasColumnName("dimensions");
            entity.Property(e => e.DiscountPercentage)
                .HasDefaultValueSql("'0'::real")
                .HasColumnName("discount_percentage");
            entity.Property(e => e.FormatId).HasColumnName("format_id");
            entity.Property(e => e.ImageUrl)
                .HasDefaultValueSql("''::text")
                .HasColumnName("image_url");
            entity.Property(e => e.Isbn13)
                .HasDefaultValueSql("''::text")
                .HasColumnName("isbn13");
            entity.Property(e => e.ItemWeight).HasColumnName("item_weight");
            entity.Property(e => e.LanguageCode)
                .HasDefaultValueSql("''::text")
                .HasColumnName("language_code");
            entity.Property(e => e.NumPages)
                .HasDefaultValueSql("'0'::bigint")
                .HasColumnName("num_pages");
            entity.Property(e => e.Price)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("price");
            entity.Property(e => e.PublicationDay)
                .HasDefaultValueSql("'0'::smallint")
                .HasColumnName("publication_day");
            entity.Property(e => e.PublicationMonth)
                .HasDefaultValueSql("'0'::smallint")
                .HasColumnName("publication_month");
            entity.Property(e => e.PublicationYear)
                .HasDefaultValueSql("'0'::smallint")
                .HasColumnName("publication_year");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.RatingsCount)
                .HasDefaultValueSql("'0'::bigint")
                .HasColumnName("ratings_count");
            entity.Property(e => e.Title)
                .HasDefaultValueSql("''::text")
                .HasColumnName("title");
            entity.Property(e => e.TitleWithoutSeries)
                .HasDefaultValueSql("''::text")
                .HasColumnName("title_without_series");
            entity.Property(e => e.Url)
                .HasDefaultValueSql("''::text")
                .HasColumnName("url");

            entity.HasOne(d => d.Format).WithMany(p => p.Books)
                .HasForeignKey(d => d.FormatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("books_format_id_fkey");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("books_publisher_id_fkey");
        });

        modelBuilder.Entity<BookFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_formats_pkey");

            entity.ToTable("book_formats");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("''::text")
                .HasColumnName("name");
        });

        modelBuilder.Entity<BookGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_genres_pkey");

            entity.ToTable("book_genres");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");

            entity.HasOne(d => d.Book).WithMany(p => p.BookGenres)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("book_genres_book_id_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.BookGenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("book_genres_genre_id_fkey");
        });

        modelBuilder.Entity<BookPublisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_publishers_pkey");

            entity.ToTable("book_publishers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("''::text")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}