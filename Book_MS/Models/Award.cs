using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Book_MS.Models
{
    public class Award
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int AwardId { get; set; }
        public required string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
