using System.ComponentModel.DataAnnotations;

namespace ReviewSystem.Dtos
{
    public class CreateAdjective
    {
        [Required(ErrorMessage = "Topic is required!", AllowEmptyStrings = false)]
        public required string Topic { get; set; }

        [Required(ErrorMessage = "Text is required!", AllowEmptyStrings = false)]
        public required string Text { get; set; }

        [Required(ErrorMessage = "ReviewId is required!", AllowEmptyStrings = false)]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "IsPositive is required!", AllowEmptyStrings = false)]
        public bool IsPositive { get; set; }
    }
}
