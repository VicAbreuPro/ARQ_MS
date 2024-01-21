using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReviewSystem.Models
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int ReviewId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int BookId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Adjective> Adjectives { get; set; } = new List<Adjective>();
    }
}
