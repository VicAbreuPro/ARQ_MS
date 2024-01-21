using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Book_MS.Models
{
    public class Shelf
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int ShelfId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int UserId { get; set; }
        public List<Book> Books { get; set; }
    }
}
