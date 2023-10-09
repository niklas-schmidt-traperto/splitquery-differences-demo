namespace PoCSplitQueries.Models;

public class Page
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Chapter Chapter { get; set; }
    public Book Book { get; set; }
}