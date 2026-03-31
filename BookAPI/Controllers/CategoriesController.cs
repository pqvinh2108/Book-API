using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly BookDbContext _context;

    public CategoriesController(BookDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var cats = await _context.Categories.Select(c => new { c.CategoryID, c.CategoryName }).ToListAsync();
        return Ok(cats);
    }
}
