using System.ComponentModel.DataAnnotations;

namespace Book_MS.Dtos
{
    public class UpdatedBook
    {
        [Required(ErrorMessage = "ID is required!")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required!", AllowEmptyStrings = false)]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Year is required!")]
        public int Year { get; set; }

        [Required(ErrorMessage = "AuthorId is required!")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Status is required!")]
        public int IsActive { get; set; }
    }
}
