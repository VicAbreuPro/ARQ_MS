using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Book_MS.Models
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int AuthorId { get; set; }
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Biography { get; set; }
        public required string Country { get; set; }
        public int IsActive { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
