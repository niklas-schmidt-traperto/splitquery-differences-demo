using Microsoft.EntityFrameworkCore;
using PoCSplitQueries.Database;
using PoCSplitQueries.Services;

// TODO: Set up
var context = new BookContext(string.Empty, MariaDbServerVersion.LatestSupportedServerVersion);
var bookService = new BookService(context);

// Comment out after first run to increase speed.
await bookService.PrepareContextAsync(30_000, CancellationToken.None);

// Shuffle & filter book ids to prevent caching on the dbms
var bookIds = (await bookService.LoadBookIdsAsync(CancellationToken.None))
    .OrderBy(_ => Guid.NewGuid())
    .Take(10_000)
    .ToList();

// Try LoadBooksSingleQueryAsync to compare results. Esp. setting skip > 0 messes with split query results.
var results = await bookService.LoadBooksSplitQueryAsync(30_000, 1, bookIds, CancellationToken.None);

var bookCounts = results
    .Select(b => (
            chapterCount: b.Chapters.Count,
            pageSum: b.Chapters.Sum(c => c.Pages.Count),
            authorCount: b.Authors.Count
        )
    )
    .ToList();

Console.WriteLine();

Console.WriteLine($"{bookCounts.Count} Books contain avg. {bookCounts.Select(b => b.chapterCount).Average()} Chapters.");
Console.WriteLine($"Min: {bookCounts.Select(b => b.chapterCount).Min()}, Max: {bookCounts.Select(b => b.chapterCount).Max()} Chapters.");

Console.WriteLine($"{bookCounts.Count} Books contain avg. {bookCounts.Select(b => b.pageSum).Average()} Pages.");
Console.WriteLine($"Min: {bookCounts.Select(b => b.pageSum).Min()}, Max: {bookCounts.Select(b => b.pageSum).Max()} Pages.");

Console.WriteLine($"{bookCounts.Count} Books contain avg. {bookCounts.Select(b => b.authorCount).Average()} Authors.");
Console.WriteLine($"Min: {bookCounts.Select(b => b.authorCount).Min()}, Max: {bookCounts.Select(b => b.authorCount).Max()} Authors.");
