using Microsoft.AspNetCore.Mvc;
using Book_MS.Repository;
using Newtonsoft.Json;
using Book_MS.Dtos;
using Book_MS.Models;
using Microsoft.AspNetCore.Authorization;

namespace Book_MS.Controllers
{
    [Route("book")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly AuthorRepository _authorRepository;
        private readonly BookRepository _bookRepository;

        public BookController(AuthorRepository authorRepository, BookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet("/allBooks")]
        public ActionResult GetAll()
        {
            return Ok(JsonConvert.SerializeObject(_bookRepository.GetBooks()));
        }

        [HttpGet("book/{bookId}")]
        public ActionResult GetBook(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);

            if (book == null)
            {
                return NotFound("Book not found!");
            }

            return Ok(JsonConvert.SerializeObject(book));
        }

        [HttpGet("book/author/{authorId}")]
        public ActionResult GetBooksByAuthor(int authorId)
        {
            var bookList = _bookRepository.GetBooksByAuthor(authorId);

            if(bookList == null || bookList.Count == 0)
            {
                return NotFound("Any available book from this Author");
            }

            return Ok(JsonConvert.SerializeObject(bookList));
        }

        [HttpPost("addBook")]
        [Authorize(Roles = "admin, author")]
        public ActionResult SubmitBook([FromBody] CreateBook book)
        {
            // Return a BadRequest if the object ivalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check Author ID
            if(_authorRepository.GetAuhtorById(book.AuthorId) == null)
            {
                return BadRequest("Invalid Author!");
            }

            // Create a new Book object to insert on DB
            var affectedRows = _bookRepository.CreateBook( new Book
            {
                Title = book.Title,
                Year = book.Year,
                AuthorId = book.AuthorId,
                IsActive = 1
            });

            if(affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(201, "Book submited!");
        }

        [HttpPut("updateBook")]
        [Authorize(Roles = "admin, author")]
        public ActionResult UpdateBook([FromBody] UpdatedBook book)
        {
            // Return a BadRequest with the wrong property if the object is not correct
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check Book ID
            if(_bookRepository.GetBookById(book.BookId) == null)
            {
                return BadRequest("Invalid Book ID");
            }

            // Check Author ID
            if (_authorRepository.GetAuhtorById(book.AuthorId) == null)
            {
                return BadRequest("Invalid Author!");
            }

            var updatedBook = new Book
            {
                BookId = book.BookId,
                Title = book.Title,
                Year = book.Year,
                AuthorId = book.AuthorId,
                IsActive = book.IsActive
            };

            int affectedRow = _bookRepository.UpdateBook(updatedBook);

            if(affectedRow == 0)
            {
                return StatusCode(500, "Server Error");
            }

            return Ok("The book with ID: " + book.BookId + " has been updated");
        }

        [HttpPost("add/{bookId}/{awardId}")]
        [Authorize(Roles = "admin")]
        public ActionResult AddBooksAward(int bookId, int awardId)
        {
            if (bookId == 0 || _bookRepository.GetBookById(bookId) == null)
            {
                return BadRequest("Invalid Book!");
            }

            if (awardId == 0)
            {
                return BadRequest("Must have Award ID!");
            }

            int affectedRows = _bookRepository.AddBookAward(bookId, awardId);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error!");
            }

            return Ok("Awards added on book  " + bookId + " !");
        }

        [HttpPut("remove/{bookId}/{awardId}")]
        [Authorize(Roles = "admin")]
        public ActionResult RemoveBooksAward(int bookId, int awardId)
        {
            if (bookId == 0 || _bookRepository.GetBookById(bookId) == null)
            {
                return BadRequest("Invalid Book!");
            }

            if (awardId == 0)
            {
                return BadRequest("Must have Award ID!");
            }

            int affectedRows = _bookRepository.RemoveBookAward(bookId, awardId);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error!");
            }

            return Ok("Awards removed from book " + bookId + " !");
        }

        [HttpPost("add/{bookId}/genre")]
        [Authorize(Roles = "admin, author")]
        public ActionResult AddBooksGenre(int bookId, [FromBody] List<int> genreIds)
        {
            if (bookId == 0 || _bookRepository.GetBookById(bookId) == null)
            {
                return BadRequest("Invalid Book!");
            }

            if (genreIds.Count == 0)
            {
                return BadRequest("Must have at least one Genre ID!");
            }

            int affectedRows = _bookRepository.AddBookGenre(bookId, genreIds);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error!");
            }

            return Ok("Genres added to the book " + bookId + " !");
        }

        [HttpPut("remove/{bookId}/genre")]
        [Authorize(Roles = "admin, author")]
        public ActionResult RemoveBooksGenre(int bookId, [FromBody] List<int> genreIds)
        {
            if (bookId == 0 || _bookRepository.GetBookById(bookId) == null)
            {
                return BadRequest("Invalid Book!");
            }

            if (genreIds.Count == 0)
            {
                return BadRequest("Must have at least one Genre ID!");
            }

            int affectedRows = _bookRepository.RemoveBookGenre(bookId, genreIds);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error!");
            }

            return Ok("Genres removed from book " + bookId + " !");
        }


        [HttpDelete("remove/{bookId}")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteBook(int bookId)
        {
            // Check Book ID
            if (_bookRepository.GetBookById(bookId) == null)
            {
                return BadRequest("Invalid Book");
            }

            int affectedRows = _bookRepository.DeleteBook(bookId);

            if(affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return NoContent();
        }
    }
}
