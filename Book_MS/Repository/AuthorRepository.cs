using Book_MS.Data;
using Book_MS.Models;

namespace Book_MS.Repository
{
    public class AuthorRepository
    {
        private readonly DataContext _dataContext;

        public AuthorRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Author? GetAuhtorById(int authorId)
        {
            return _dataContext.Authors.Find(authorId);
        }

        public List<Author> GetAll()
        {
            return _dataContext.Authors.ToList();
        }

        public int CreateAuthor(Author author)
        {
            _dataContext.Authors.Add(author);

            return _dataContext.SaveChanges();
        }

        public int UpdateAuthor(Author updatedAuthor)
        {
            var author = _dataContext.Authors.Find(updatedAuthor.AuthorId);

            if(author != null)
            {
                author.Name = updatedAuthor.Name;
                author.BirthDate = updatedAuthor.BirthDate;
                author.Biography = updatedAuthor.Biography;
                author.Country = updatedAuthor.Country;
                author.IsActive = updatedAuthor.IsActive;
            }

            return _dataContext.SaveChanges();
        }

        public int DeleteAuthor(int authorId)
        {
            var author = _dataContext.Authors.Find(authorId);

            if (author != null)
            {
                author.IsActive = 0;
            }

            return _dataContext.SaveChanges();
        }
    }
}
