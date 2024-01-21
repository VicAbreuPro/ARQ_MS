using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReviewSystem.Models
{
    public class Adjective
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int AdjectiveId { get; set; }
        public int ReviewId { get; set; }
        public required string Topic { get; set; }
        public required string Text { get; set; }
        public bool IsPositive { get; set; }
    }
}
