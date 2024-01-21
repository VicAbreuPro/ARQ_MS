using ReviewSystem.Data;
using ReviewSystem.Models;

namespace ReviewSystem.Repository
{
    public class ReviewRepository
    {
        private readonly DataContext _dataContext;

        public ReviewRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int AddAdjectiveToReview(int reviewId, int adjectiveId)
        {
            var review = _dataContext.Reviews.Find(reviewId);
            var adjective = _dataContext.Adjectives.Find(adjectiveId);

            if(review != null && adjective != null)
            {
                review.Adjectives.Add(adjective);
            }

            return _dataContext.SaveChanges();
        }

        public int RemoveAdjectiveToReview(int reviewId, int adjectiveId)
        {
            var review = _dataContext.Reviews.Find(reviewId);
            var adjective = _dataContext.Adjectives.Find(adjectiveId);

            if (review != null && adjective != null)
            {
                review.Adjectives.Remove(adjective);
            }

            return _dataContext.SaveChanges();
        }

        public int AddLikeToReview(int reviewId, Like like)
        {
            _dataContext.Likes.Add(like);

            var resultOfInsert = _dataContext.SaveChanges();

            if(resultOfInsert != 0)
            {
                var review = _dataContext.Reviews.Find(reviewId);

                if (review != null)
                {
                    review.Likes.Add(like);
                }
            }

            var resultOfAddLike = _dataContext.SaveChanges();

            if (resultOfAddLike != 0)
            {
                return like.LikeId;
            }

            return 0;
        }

        public int RemoveLikeFromReview(int reviewId, int likeId)
        {
            var review = _dataContext.Reviews.Find(reviewId);
            var like = _dataContext.Likes.Find(likeId);

            if (review != null && like != null)
            {
                review.Likes.Remove(like);

                _dataContext.Likes.Remove(like);
            }

            return _dataContext.SaveChanges();
        }

        public int GetReviewLikes(int reviewId)
        {
            return _dataContext.Likes.Where(lk => lk.ReviewId == reviewId).Count();
        }

        public List<Review> GetAll(int bookId)
        {
            return _dataContext.Reviews.Where( rw => rw.BookId == bookId ).ToList();
        }

        public Review? Get(int reviewId)
        {
            return _dataContext.Reviews.Find(reviewId);
        }

        public int Create(Review review)
        {
            _dataContext.Reviews.Add(review);

            return _dataContext.SaveChanges();
        }

        public int Update(int reviewId, Review review)
        {
            var reviewToUpdate = _dataContext.Reviews.Find(reviewId);

            if(reviewToUpdate != null)
            {
                reviewToUpdate.Title = review.Title;
                reviewToUpdate.Description = review.Description;
            }

            return _dataContext.SaveChanges();
        }

        public int Delete(int reviewId)
        {
            var review = _dataContext.Reviews.Find(reviewId);

            if(review != null)
            {
                _dataContext.Reviews.Remove(review);
            }

            return _dataContext.SaveChanges();
        }
    }
}
