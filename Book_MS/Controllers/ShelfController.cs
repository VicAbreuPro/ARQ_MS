using Book_MS.Dtos;
using Book_MS.Models;
using Book_MS.Repository;
using Book_MS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace Book_MS.Controllers
{
    [Route("Shelf")]
    [ApiController]
    public class ShelfController : Controller
    {
        private readonly ShelfRepository _shelfRepository;
        private readonly IConfiguration _configuration;

        public ShelfController(ShelfRepository shelfRepository, IConfiguration configuration)
        {
            _shelfRepository = shelfRepository;
            _configuration = configuration;
        }

        [HttpGet("user")]
        [Authorize]
        public ActionResult GetShelfByUser()
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
            int userId = ts.GetUserIdFromJwtToken(token);

            var shelf = _shelfRepository.GetByUser(userId);

            if(shelf == null)
            {
                return NotFound("Shelf Not Found");
            }

            return Ok(JsonConvert.SerializeObject(shelf));
        }

        [HttpGet("userShelfs")]
        [Authorize]
        public ActionResult GetAllShelfByUser()
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
            int userId = ts.GetUserIdFromJwtToken(token);

            var shelfList = _shelfRepository.GetAllShelf(userId);

            return Ok(JsonConvert.SerializeObject(shelfList));
        }

        [HttpPost("addShelf")]
        [Authorize]
        public ActionResult CreateShelf([FromBody] CreateShelf shelf)
        {
            // Return a BadRequest with the wrong property if the object is not correct
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            int userId = ts.GetUserIdFromJwtToken(token);

            var newShelf = new Shelf
            {
                Name = shelf.Name,
                Description = shelf.Description,
                UserId = userId,
            };

            int affectedRows = _shelfRepository.CreateShelf(newShelf);

            if(affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error!");
            }

            return StatusCode(201, "Shelf " + shelf.Name + " Created!");
        }

        [HttpPost("add/{shelfId}/book")]
        [Authorize]
        public ActionResult AddBooksToShelf(int shelfId, [FromBody] List<int> bookIds)
        {
            if(shelfId == 0 || _shelfRepository.GetShelf(shelfId) == null)
            {
                return BadRequest("Invalid Shelf ID");
            }

            if(bookIds.Count == 0)
            {
                return BadRequest("Must have at least one Book ID!");
            }

            int affectedRows = _shelfRepository.AddBookToShelf(shelfId, bookIds);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error!");
            }

            return Ok("Books added on  shelf  " + shelfId + " !");
        }

        [HttpPut("remove/{shelfId}/book")]
        [Authorize]
        public ActionResult RemoveBooksToShelf(int shelfId, [FromBody] List<int> bookIds)
        {
            if (shelfId == 0 || _shelfRepository.GetShelf(shelfId) == null)
            {
                return BadRequest("Invalid Shelf ID");
            }

            if (bookIds.Count == 0)
            {
                return BadRequest("Must have at least one Book ID!");
            }

            int affectedRows = _shelfRepository.RemoveBookFromShelf(shelfId, bookIds);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error!");
            }

            return Ok("Books removed from shelf  " + shelfId + " !");
        }

        [HttpPut("updateShelf")]
        [Authorize]
        public ActionResult UpdateShelf([FromBody] UpdateShelf shelf)
        {
            // Return a BadRequest with the wrong property if the object is not correct
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(_shelfRepository.GetShelf(shelf.ShelfId) == null)
            {
                return BadRequest("Invalid Shelf ID");
            }

            var updatedShelf = new Shelf
            {
                ShelfId = shelf.ShelfId,
                Name = shelf.Name,
                Description = shelf.Description
            };

            int affectedRows = _shelfRepository.UpdateShelf(updatedShelf);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error!");
            }

            return Ok("Shelf " + shelf.ShelfId + " updated!");
        }

        [HttpDelete("remove/{shelfId}")]
        [Authorize]
        public ActionResult DeleteShelf(int shelfId)
        {
            int affectedRows = _shelfRepository.DeleteShelf(shelfId);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error!");
            }

            return NoContent();
        }
    }
}
