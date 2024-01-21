using Book_MS.Dtos;
using Book_MS.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Book_MS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Book_MS.Services;

namespace Book_MS.Controllers
{
    [Route("Author")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly AuthorRepository _authorRepository;
        private readonly IConfiguration _configuration;

        // Constructor
        public AuthorController(AuthorRepository authorRepository, IConfiguration configuration)
        {
            _authorRepository = authorRepository;
            _configuration = configuration;
        }

        [HttpGet("allAuthors")]
        [Authorize]
        public ActionResult GetAll()
        {
            return Ok(JsonConvert.SerializeObject(_authorRepository.GetAll()));
        }

        [HttpGet("author/{authorId}")]
        [Authorize]
        public ActionResult GetAuthor(int authorId)
        {
            var author = _authorRepository.GetAuhtorById(authorId);

            if(author == null)
            {
                return NotFound("Author not found!");
            }

            return Ok(JsonConvert.SerializeObject(author));
        }

        [HttpPost("addAuthor")]
        [Authorize(Roles = "author, admin")]
        public ActionResult CreateAuthor([FromBody] CreateAuthor author)
        {
            // Return a BadRequest with the wrong property if the object is not correct
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var affectedRows = _authorRepository.CreateAuthor(new Author
            {
                Name = author.Name,
                BirthDate = author.BirthDate.ToDateTime(TimeOnly.MinValue),
                Biography = author.Biography,
                Country = author.Country,
                IsActive = 1
            });

            if(affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(201, "Author Created!");
        }

        [HttpPut("updateAuthor")]
        [Authorize(Roles = "author, admin")]
        public ActionResult UpdateAuthor([FromBody] UpdateAuthor author)
        {
            // Return a BadRequest with the wrong property if the object is not correct
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(_authorRepository.GetAuhtorById(author.AuthorId) == null)
            {
                return BadRequest("Invalid Author ID");
            }

            var updatedAuthor = new Author
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                BirthDate = author.BirthDate.ToDateTime(TimeOnly.MinValue),
                Biography = author.Biography,
                Country = author.Country,
                IsActive = author.IsActive
            };

            int affectedRows = _authorRepository.UpdateAuthor(updatedAuthor);

            if(affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return Ok("The Author with ID: " + author.AuthorId + " has been updated");
        }

        [HttpDelete("remove/{authorId}")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteAuthor(int authorId)
        {
            if (_authorRepository.GetAuhtorById(authorId) == null)
            {
                return BadRequest("Invalid Author ID");
            }

            int affectedRows = _authorRepository.DeleteAuthor(authorId);

            if(affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return NoContent();
        }

        [HttpDelete("removeMeAuthor")]
        [Authorize(Roles = "author")]
        public ActionResult DeleteMeAuthor()
        {
            var token = "";
            TokenService ts;
            ts = new TokenService(_configuration);

            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                token = authHeader.ToString().Replace("Bearer ", "");
            }

            if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeaderAux))
            {
                return BadRequest("Invalid Token");
            }

            // Get userId from token
            int authorId = ts.GetUserIdFromJwtToken(token);

            int affectedRows = _authorRepository.DeleteAuthor(authorId);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return NoContent();
        }
    }
}
