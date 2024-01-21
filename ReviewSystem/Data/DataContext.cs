using Microsoft.EntityFrameworkCore;
using ReviewSystem.Models;

/**
 * Data Context class
 * Here is configurade all the tables to be used with EntityFramework
 */
namespace ReviewSystem.Data
{
    public class DataContext: DbContext
    {
        // Default constructor
        public DataContext() { }

        // Constructor to create an instance of DataContext class with specifc configuration
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // Set Table Reviews
        public DbSet<Review> Reviews { get; set; }

        // Set Table Likes
        public DbSet<Like> Likes { get; set; }

        // Set Table Adjectives
        public DbSet<Adjective> Adjectives { get; set; }
    }
}
