namespace PoCSplitQueries.Models;

public class Book
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Chapter> Chapters { get; set; }
    public List<Page> Pages { get; set; }
    public List<Author> Authors { get; set; }
}