using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReviewSystem.Dtos;
using ReviewSystem.Models;
using ReviewSystem.Repository;

namespace ReviewSystem.Controllers
{
    [Route("Review")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly ReviewRepository _reviewRepository;

        public ReviewController(ReviewRepository reviewRepository )
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet("bookReview/{bookId}")]
        [Authorize]
        public ActionResult GetReviewsByBook(int bookId)
        {
            var reviewList = _reviewRepository.GetAll(bookId);

            return Ok(JsonConvert.SerializeObject(reviewList));
        }

        [HttpGet("likesCount/{reviewId}")]
        [Authorize]
        public ActionResult GetReviewsLikeCount(int reviewId)
        {
            var count = _reviewRepository.GetReviewLikes(reviewId);

            return Ok(count);
        }

        [HttpPost("newReview")]
        [Authorize]
        public ActionResult CreateReview([FromBody] CreateReview review)
        {
            // Return a BadRequest if the object ivalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var affectedRows = _reviewRepository.Create(new Review
            {
                Title = review.Title,
                Description = review.Description,
                BookId = review.BookId,
            });

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(201, "Review submited!");
        }

        [HttpPut("updateReview/{reviewId}")]
        [Authorize]
        public ActionResult UpdateReview(int reviewId, [FromBody] CreateReview reviewToUpdate)
        {
            // Return a BadRequest if the object ivalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check review
            var review = _reviewRepository.Get(reviewId);

            if (review == null)
            {
                return BadRequest("Invalid Review");
            }

            var affectedRows = _reviewRepository.Update(reviewId, new Review
            {
                Title = reviewToUpdate.Title,
                Description = reviewToUpdate.Description,
                BookId = reviewToUpdate.BookId,
            });

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return Ok("Review Updated!");
        }

        [HttpPost("likeReview/{reviewId}/{userId}")]
        [Authorize]
        public ActionResult LikeReview(int reviewId, int userId)
        {
            // check review
            var review = _reviewRepository.Get(reviewId);

            if(review != null)
            {
                return BadRequest("Invalid Review");
            }

            var like = _reviewRepository.AddLikeToReview(reviewId, new Like
            {
                UserId = userId,
                ReviewId = reviewId
            });

            if(like == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(201, "Like with ID: " + like + "was submited");
        }

        [HttpDelete("removeLike/{likeId}/{reviewId}")]
        [Authorize]
        public ActionResult RemoveLike(int likeId, int reviewId)
        {
            // check review
            var review = _reviewRepository.Get(reviewId);

            if (review != null)
            {
                return BadRequest("Invalid Review");
            }

            var affectedRows = _reviewRepository.RemoveLikeFromReview(reviewId, likeId);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(204, "Like Removed");
        }

        [HttpDelete("removeReview/{reviewId}")]
        [Authorize]
        public ActionResult RemoveReview(int reviewId)
        {
            // check review
            var review = _reviewRepository.Get(reviewId);

            if (review != null)
            {
                return BadRequest("Invalid Review");
            }

            var affectedRows = _reviewRepository.Delete(reviewId);

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            return StatusCode(204, "Review Removed");
        }
    }
}
