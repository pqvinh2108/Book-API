public class Book
{
    public int BookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    public string ImageFileName { get; set; }
    public int CategoryID { get; set; }
    public virtual Category Category { get; set; }
}