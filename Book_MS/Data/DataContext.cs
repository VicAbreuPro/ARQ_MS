using Microsoft.EntityFrameworkCore;
using Book_MS.Models;

/**
 * Data Context class
 * Here is configurade all the tables to be used with EntityFramework
 */
namespace Book_MS.Data
{
    public class DataContext: DbContext
    {
        // Default constructor
        public DataContext() { }

        // Constructor to create an instance of DataContext class with specifc configuration
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // Set Table Books
        public DbSet<Book> Books { get; set; }

        // Set Table Authors
        public DbSet<Author> Authors { get; set; }

        // Set Table Shelf
        public DbSet<Shelf> Shelf { get; set; }

        // Set table Awards
        public DbSet<Award> Awards { get; set; }

        // Set table Genre
        public DbSet<Genre> Genres { get; set; }
    }
}
