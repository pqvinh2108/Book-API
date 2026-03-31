using Microsoft.EntityFrameworkCore;

public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .Property(b => b.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryID = 1, CategoryName = "Programming" },
            new Category { CategoryID = 2, CategoryName = "Networking" },
            new Category { CategoryID = 3, CategoryName = "Database" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book { BookID = 1, Title = "C# for Beginners", Author = "John Doe", Price = 50.00m, CategoryID = 1, ImageFileName = "csharp.jpg" },
            new Book { BookID = 2, Title = "TCP/IP Illustrated", Author = "W. Richard Stevens", Price = 65.50m, CategoryID = 2, ImageFileName = "tcpip.jpg" }
        );
    }
}