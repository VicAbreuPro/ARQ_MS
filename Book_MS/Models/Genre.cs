using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Book_MS.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int GenreId { get; set; }
        public required string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
