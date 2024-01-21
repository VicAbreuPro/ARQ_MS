using System.ComponentModel.DataAnnotations;

namespace ReviewSystem.Dtos
{
    public class CreateReview
    {
        [Required(ErrorMessage = "Title is required!", AllowEmptyStrings = false)]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Description is required!", AllowEmptyStrings = false)]
        public required string Description { get; set; }

        [Required(ErrorMessage = "BookId is required!", AllowEmptyStrings = false)]
        public int BookId { get; set; }
    }
}
