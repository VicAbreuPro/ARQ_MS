using System.ComponentModel.DataAnnotations;

namespace Book_MS.Dtos
{
    public class UpdateShelf
    {
        [Required(ErrorMessage = "ID is required!")]
        public int ShelfId { get; set; }

        [Required(ErrorMessage = "Name is required!", AllowEmptyStrings = false)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        public required string Description { get; set; }
    }
}
