namespace PoCSplitQueries.Models;

public class Chapter
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Book Book { get; set; }
    public List<Page> Pages { get; set; }
}