using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoCSplitQueries.Models;

namespace PoCSplitQueries.Database;

public class BookContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseMySql(
            "Server=XXX;Database=XXX;Uid=XXX;Pwd=XXX;",
            MariaDbServerVersion.LatestSupportedServerVersion
        );

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}