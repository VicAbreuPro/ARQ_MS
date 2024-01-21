using Book_MS.Models;
using Book_MS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Book_MS.Controllers
{
    [Route("Award")]
    [ApiController]
    public class AwardController : Controller
    {
        private readonly AwardRepository _awardRepository;

        public AwardController(AwardRepository awardRepository)
        {
            _awardRepository = awardRepository;
        }

        [HttpGet("detail/{awardId}")]
        [Authorize]
        public ActionResult GetAward(int awardId)
        {
            var award = _awardRepository.Get(awardId);

            if(award == null)
            {
                return BadRequest("Invalid Award!");
            }

            return Ok(JsonConvert.SerializeObject(award));
        }

        [HttpGet("allAwards")]
        [Authorize]
        public ActionResult GetAllAwards()
        {
            var awardList = _awardRepository.GetAll();

            return Ok(JsonConvert.SerializeObject(awardList));
        }

        [HttpPost("addAward")]
        [Authorize(Roles = "admin")]
        public ActionResult CreateAward([FromBody] string name)
        {
            if(name == null || name == string.Empty)
            {
                return BadRequest("Invalid Award name!");
            }

            var award = new Award { Name = name };

            int affectedRows = _awardRepository.CreateAward(award);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(201, "Award created!");
        }

        [HttpPut("update/{awardId}")]
        [Authorize(Roles = "admin")]
        public ActionResult UpdateAward(int awardId, [FromBody] string name)
        {
            if(_awardRepository.Get(awardId) == null)
            {
                return BadRequest("Invalid Award!");
            }

            if (name == null || name == string.Empty)
            {
                return BadRequest("Invalid Award name!");
            }

            int affectedRows = _awardRepository.UpdateAward(awardId, name);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return Ok("Award Updated!");
        }

        [HttpDelete("remove/{awardId}")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteAward(int awardId)
        {
            if (_awardRepository.Get(awardId) == null)
            {
                return BadRequest("Invalid Award!");
            }

            int affectedRows = _awardRepository.DeleteAward(awardId);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return NoContent();
        }
    }
}
