using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReviewSystem.Models
{
    public class Like
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int LikeId { get; set; }
        public int UserId { get; set; }
        public int ReviewId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
