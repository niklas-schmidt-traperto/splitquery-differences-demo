using Microsoft.EntityFrameworkCore;
using PoCSplitQueries.Database;
using PoCSplitQueries.Models;

namespace PoCSplitQueries.Services;

public class BookService
{
    private readonly BookContext _bookContext;

    public BookService(
        BookContext bookContext
    )
    {
        _bookContext = bookContext;
    }

    public async Task PrepareContextAsync(CancellationToken cancellationToken)
    {
        /* Start with fresh data */
        var existingBooks = await _bookContext.Books.ToListAsync(cancellationToken);
        _bookContext.Books.RemoveRange(existingBooks);
        await _bookContext.SaveChangesAsync(cancellationToken);

        var existingAuthors = await _bookContext.Authors.ToListAsync(cancellationToken);
        _bookContext.Authors.RemoveRange(existingAuthors);
        await _bookContext.SaveChangesAsync(cancellationToken);

        for (var i = 1; i <= 1000; i++)
        {
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = $"Author #{i}",
                Books = new List<Book>()
            };

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Name = $"Book #{i}",
                Chapters = new List<Chapter>(),
                Pages = new List<Page>(),
                Authors = new List<Author> { author }
            };

            _bookContext.Authors.Add(author);
            _bookContext.Books.Add(book);
            book.Authors.Add(author);

            for (var j = 1; j <= 5; j++)
            {
                var chapter = new Chapter
                {
                    Id = Guid.NewGuid(),
                    Name = $"Book #{i}, Chapter #{j}",
                    Book = book,
                    Pages = new List<Page>()
                };
                book.Chapters.Add(chapter);
                _bookContext.Chapters.Add(chapter);

                for (var k = 1; k <= 10; k++)
                {
                    var page = new Page
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Book #{i}, Chapter #{j}, Page #{k}",
                        Chapter = chapter,
                        Book = book
                    };
                    chapter.Pages.Add(page);
                    book.Pages.Add(page);
                    _bookContext.Pages.Add(page);
                }
            }
        }

        await _bookContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<Book>> LoadBooksAsync(
        int limit,
        int skip,
        CancellationToken cancellationToken
    )
    {
        return await _bookContext.Books
           .OrderBy(b => b.Name)
           .Take(limit)
           .Skip(skip)
           .AsSplitQuery()
           .Include(b => b.Chapters)
           .ThenInclude(c => c.Pages)
           .Include(b => b.Authors)
           .ToListAsync(cancellationToken);
    }
}