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

    private readonly string _connectionString;
    private readonly ServerVersion _serverVersion;

    public BookContext(
        string connectionString,
        ServerVersion serverVersion
    )
    {
        _connectionString = connectionString;
        _serverVersion = serverVersion;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseMySql(_connectionString, _serverVersion);
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}