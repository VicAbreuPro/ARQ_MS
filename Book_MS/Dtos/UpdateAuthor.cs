using System.ComponentModel.DataAnnotations;

namespace Book_MS.Dtos
{
    public class UpdateAuthor
    {
        [Required(ErrorMessage = "ID is required!", AllowEmptyStrings = false)]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Name is required!", AllowEmptyStrings = false)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "BirthDate is required!", AllowEmptyStrings = false)]
        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Biography is required!", AllowEmptyStrings = false)]
        public required string Biography { get; set; }

        [Required(ErrorMessage = "Country is required!", AllowEmptyStrings = false)]
        public required string Country { get; set; }

        [Required(ErrorMessage = "Status is required!", AllowEmptyStrings = false)]
        public int IsActive { get; set; }
    }
}
