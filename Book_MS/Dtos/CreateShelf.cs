using System.ComponentModel.DataAnnotations;

namespace Book_MS.Dtos
{
    public class CreateShelf
    {
        [Required(ErrorMessage = "Name is required!", AllowEmptyStrings = false)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        public required string Description { get; set; }
    }
}
