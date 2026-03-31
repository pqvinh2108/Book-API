using Microsoft.AspNetCore.Http;

public class BookDTO
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public IFormFile? ImageFile { get; set; }
    public int CategoryID { get; set; }
}
