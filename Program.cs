using PoCSplitQueries.Database;
using PoCSplitQueries.Services;

var context = new BookContext();
var bookService = new BookService(context);

await bookService.PrepareContextAsync(CancellationToken.None);

var results = await bookService.LoadBooksAsync(10, 0, CancellationToken.None);

var bookCounts = results
   .Select(b => b.Chapters.Count)
   .ToList();

var authorCounts = results
   .Select(b => b.Authors.Count)
   .ToList();

Console.WriteLine($"{bookCounts.Count} Books contain avg. {bookCounts.Average()} Chapters.");
Console.WriteLine($"Min: {bookCounts.Min()}, Max: {bookCounts.Max()} Chapters.");

Console.WriteLine($"{authorCounts.Count} Books contain avg. {authorCounts.Average()} Authors.");
Console.WriteLine($"Min: {authorCounts.Min()}, Max: {authorCounts.Max()} Authors.");