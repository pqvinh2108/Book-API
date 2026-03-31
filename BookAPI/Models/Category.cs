public class Category
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public virtual ICollection<Book> Books { get; set; }
}