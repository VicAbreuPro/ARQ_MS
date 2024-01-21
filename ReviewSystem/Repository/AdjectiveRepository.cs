using ReviewSystem.Data;
using ReviewSystem.Models;

namespace ReviewSystem.Repository
{
    public class AdjectiveRepository
    {
        private readonly DataContext _dataContext;

        public AdjectiveRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int CreateAdjective(Adjective adjective)
        {
            _dataContext.Adjectives.Add(adjective);

            return _dataContext.SaveChanges();
        }

        public Adjective? GetOne(int adjectiveId)
        {
            return _dataContext.Adjectives.Find(adjectiveId);
        }

        public List<Adjective> GetAll()
        {
            return _dataContext.Adjectives.ToList();
        }

        public int Update(int adjectiveId, Adjective adjective)
        {
            var adjectiveToUpdate = _dataContext.Adjectives.Find(adjectiveId);

            if(adjectiveToUpdate != null)
            {
                adjectiveToUpdate.Text = adjective.Text;
                adjectiveToUpdate.Topic = adjective.Topic;
                adjectiveToUpdate.IsPositive = adjective.IsPositive;
            }

            return _dataContext.SaveChanges();
        }

        public int Delete(int adjectiveId)
        {
            var adjective = _dataContext.Adjectives.Find(adjectiveId);

            if(adjective != null)
            {
                _dataContext.Adjectives.Remove(adjective);
            }

            return _dataContext.SaveChanges();
        }
    }
}
