using Book_MS.Data;
using Book_MS.Models;

namespace Book_MS.Repository
{
    public class AwardRepository
    {
        private readonly DataContext _dataContext;

        public AwardRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<Award> GetAll()
        {
            return _dataContext.Awards.ToList();
        }

        public Award? Get(int awardId)
        {
            return _dataContext.Awards.Find(awardId);
        }

        public List<Award> GetAwardsByBook(int bookId)
        {
            return _dataContext.Awards.Where(aw => aw.Books.Any(book => book.BookId == bookId)).ToList();
        }

        public int CreateAward (Award award)
        {
            _dataContext.Awards.Add(award);

            return _dataContext.SaveChanges();
        }

        public int UpdateAward (int awardId, string name)
        {
            var award = _dataContext.Awards.Find(awardId);

            if(award != null)
            {
                award.Name = name;
            }

            return _dataContext.SaveChanges();
        }

        public int DeleteAward (int awardId)
        {
            var award = _dataContext.Awards.Find(awardId);

            if(award != null)
            {
                _dataContext.Awards.Remove(award);
            }

            return _dataContext.SaveChanges();
        }
    }
}
