using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ReviewSystem.Dtos;
using ReviewSystem.Models;
using ReviewSystem.Repository;

namespace ReviewSystem.Controllers
{
    public class AdjectiveController : Controller
    {
        private readonly AdjectiveRepository _adjectiveRepository;
        private readonly ReviewRepository _reviewRepository;

        public AdjectiveController(AdjectiveRepository adjectiveRepository, ReviewRepository reviewRepository)
        {
            _adjectiveRepository = adjectiveRepository;
            _reviewRepository = reviewRepository;
        }

        [HttpPost("newAdjective")]
        [Authorize]
        public ActionResult CreateAdjective([FromBody] CreateAdjective adjective)
        {
            // Return a BadRequest if the object ivalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = _reviewRepository.Get(adjective.ReviewId);

            if (review == null)
            {
                return BadRequest("Invalid Review");
            }

            var affectedRows = _adjectiveRepository.CreateAdjective(new Adjective
            {
                Topic = adjective.Topic,
                Text = adjective.Text,
                IsPositive = adjective.IsPositive,
                ReviewId = adjective.ReviewId,
            });

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(201, "Adjective created!");
        }

        [HttpPut("updateAdjective/{adjectiveId}")]
        [Authorize]
        public ActionResult UpdateAdjective(int adjectiveId, [FromBody] CreateAdjective adjectiveToUpdate)
        {
            // Return a BadRequest if the object ivalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var affectedRows = _adjectiveRepository.Update(adjectiveId, new Adjective
            {
                Topic = adjectiveToUpdate.Topic,
                Text = adjectiveToUpdate.Text,
                IsPositive = adjectiveToUpdate.IsPositive,
                ReviewId = adjectiveToUpdate.ReviewId,
            });

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(201, "Adjective with ID: " + adjectiveId + " updated!");
        }

        [HttpGet("allAdjectives")]
        [Authorize]
        public ActionResult GetAllAdjectives()
        {
            var adjectiveList = _adjectiveRepository.GetAll();

            return Ok(JsonConvert.SerializeObject(adjectiveList));
        }

        [HttpGet("adjectiveDetail/{adjectiveId}")]
        [Authorize]
        public ActionResult GetAllAdjectives(int adjectiveId)
        {
            var adjective = _adjectiveRepository.GetOne(adjectiveId);

            return Ok(JsonConvert.SerializeObject(adjective));
        }

        [HttpDelete("removeAdjective/{adjectiveId}")]
        [Authorize]
        public ActionResult RemoveAdjective(int adjectiveId)
        {
            var adjective = _adjectiveRepository.GetOne(adjectiveId);

            if(adjective == null)
            {
                return BadRequest("Invalid Adjective");
            }

            var affectedRows = _adjectiveRepository.Delete(adjectiveId);

            if (affectedRows == 0)

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(204);
        }
    }
}
