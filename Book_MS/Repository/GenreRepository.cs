using Book_MS.Data;
using Book_MS.Models;

namespace Book_MS.Repository
{
    public class GenreRepository
    {
        private readonly DataContext _dataContext;

        public GenreRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<Genre> GetAll()
        {
            return _dataContext.Genres.ToList();
        }

        public Genre? Get(int genreId)
        {
            return _dataContext.Genres.Find(genreId);
        }

        public int CreateGenre(Genre genre)
        {
            _dataContext.Genres.Add(genre);

            return _dataContext.SaveChanges();
        }

        public int DeleteGenre(int genreId)
        {
            var genre = _dataContext.Genres.Find(genreId);

            if (genre != null)
            {
                _dataContext.Genres.Remove(genre);
            }

            return _dataContext.SaveChanges();
        }
    }
}
