using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Book_MS.Models
{
    public class Book
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int BookId { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public int AuthorId { get; set; }
        public int IsActive { get; set; }
        public List<Award> Awards { get; set; }
        public List<Shelf> Shelfs { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
