using Book_MS.Models;
using Book_MS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Book_MS.Controllers
{
    [Route("Genre")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly GenreRepository _genreRepository;

        public GenreController(GenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        [HttpGet("allGenres")]
        [Authorize]
        public ActionResult GetAllGenres()
        {
            var genreList = _genreRepository.GetAll();

            return Ok(JsonConvert.SerializeObject(genreList));
        }

        [HttpPost("addGenre")]
        [Authorize(Roles = "admin")]
        public ActionResult CreateAward([FromBody] string name)
        {
            if (name == null || name == string.Empty)
            {
                return BadRequest("Invalid Genre name!");
            }

            var genre = new Genre { Name = name };

            int affectedRows = _genreRepository.CreateGenre(genre);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(201, "Genre created!");
        }

        [HttpDelete("deleteGenre")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteGenre(int genreId)
        {
            if (_genreRepository.Get(genreId) == null)
            {
                return BadRequest("Invalid Award!");
            }

            int affectedRows = _genreRepository.DeleteGenre(genreId);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return NoContent();
        }
    }
}
