using Microsoft.EntityFrameworkCore;
using PoCSplitQueries.Database;
using PoCSplitQueries.Models;

namespace PoCSplitQueries.Services;

public class BookService
{
    private readonly BookContext _bookContext;

    public BookService(BookContext bookContext)
    {
        _bookContext = bookContext;
    }

    public async Task PrepareContextAsync(int bookCount, CancellationToken cancellationToken)
    {
        /* Start with fresh data */
        var existingBooks = await _bookContext.Books.ToListAsync(cancellationToken);
        _bookContext.Books.RemoveRange(existingBooks);
        await _bookContext.SaveChangesAsync(cancellationToken);

        var existingAuthors = await _bookContext.Authors.ToListAsync(cancellationToken);
        _bookContext.Authors.RemoveRange(existingAuthors);
        await _bookContext.SaveChangesAsync(cancellationToken);

        for (var i = 1; i <= bookCount; i++)
        {
            var author1 = new Author
            {
                Id = Guid.NewGuid(),
                Name = $"Author #{i}.1",
                Books = new List<Book>()
            };

            var author2 = new Author
            {
                Id = Guid.NewGuid(),
                Name = $"Author #{i}.2",
                Books = new List<Book>()
            };

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Name = $"Book #{i}",
                Chapters = new List<Chapter>(),
                Pages = new List<Page>(),
                Authors = new List<Author> { author1, author2 }
            };

            _bookContext.Authors.Add(author1);
            _bookContext.Authors.Add(author2);
            _bookContext.Books.Add(book);

            for (var j = 1; j <= 2; j++)
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

                for (var k = 1; k <= 5; k++)
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

    public async Task<IList<Book>> LoadBooksSplitQueryAsync(
        int limit,
        int skip,
        IEnumerable<Guid> bookIds,
        CancellationToken cancellationToken
    )
    {
        return await _bookContext.Books
            .AsSplitQuery()
            .Where(b => bookIds.Contains(b.Id))
            .Skip(skip)
            .Take(limit)
            .Include(b => b.Chapters)
            .ThenInclude(c => c.Pages)
            .Include(b => b.Authors)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<Book>> LoadBooksSingleQueryAsync(
        int limit,
        int skip,
        IEnumerable<Guid> bookIds,
        CancellationToken cancellationToken
    )
    {
        return await _bookContext.Books
            .AsSingleQuery()
            .Where(b => bookIds.Contains(b.Id))
            .Skip(skip)
            .Take(limit)
            .Include(b => b.Chapters)
            .ThenInclude(c => c.Pages)
            .Include(b => b.Authors)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Guid>> LoadBookIdsAsync(CancellationToken cancellationToken)
    {
        return await _bookContext.Books
            .Select(b => b.Id)
            .ToListAsync(cancellationToken);
    }
}