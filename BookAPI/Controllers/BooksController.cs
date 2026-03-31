using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookDbContext _context;
    private readonly IWebHostEnvironment _env;

    public BooksController(BookDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] string search = "")
    {
        var query = _context.Books.Include(b => b.Category).AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
        }

        var books = await query.Select(b => new {
            b.BookID,
            b.Title,
            b.Author,
            b.Price,
            b.ImageFileName,
            b.CategoryID,
            CategoryName = b.Category.CategoryName
        }).ToListAsync();

        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook([FromForm] BookDTO dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            Price = dto.Price,
            CategoryID = dto.CategoryID,
            ImageFileName = "" // Default value to prevent SQL NULL constraint error
        };

        if (dto.ImageFile != null && dto.ImageFile.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ImageFile.FileName);
            var filePath = Path.Combine(_env.WebRootPath, "Content", "ImageBooks", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ImageFile.CopyToAsync(stream);
            }
            book.ImageFileName = fileName;
        }

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return Ok(book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditBook(int id, [FromForm] BookDTO dto)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.Price = dto.Price;
        book.CategoryID = dto.CategoryID;

        if (dto.ImageFile != null && dto.ImageFile.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ImageFile.FileName);
            var filePath = Path.Combine(_env.WebRootPath, "Content", "ImageBooks", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ImageFile.CopyToAsync(stream);
            }
            book.ImageFileName = fileName;
        }

        await _context.SaveChangesAsync();
        return Ok(book);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Deleted" });
    }
}
