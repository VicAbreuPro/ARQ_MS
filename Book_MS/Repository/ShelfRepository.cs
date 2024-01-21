using Book_MS.Data;
using Book_MS.Models;
using System.Linq;

namespace Book_MS.Repository
{
    public class ShelfRepository
    {
        private readonly DataContext _dataContext;

        public ShelfRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Shelf? GetShelf(int shelfId)
        {
            return _dataContext.Shelf.Find(shelfId);
        }

        public List<Shelf> GetAllShelf(int userId)
        {
            return _dataContext.Shelf.Where(shelf => shelf.UserId == userId).ToList();
        }

        public Shelf? GetByUser(int userId)
        {
            return _dataContext.Shelf.Where(shelf => shelf.UserId == userId).FirstOrDefault();
        }

        public int CreateShelf(Shelf shelf)
        {
            _dataContext.Shelf.Add(shelf);

            return _dataContext.SaveChanges();
        }

        public int AddBookToShelf(int shelfId, List<int> books)
        {
            var shelf = _dataContext.Shelf.Find(shelfId);

            if(shelf !=  null)
            {
                var booksList = _dataContext.Books.Where( book => books.Contains(book.BookId)).ToList();

                foreach(Book book in booksList)
                {
                    shelf.Books.Add(book);
                }
            }

            return _dataContext.SaveChanges();
        }

        public int RemoveBookFromShelf(int shelfId, List<int> books)
        {
            var shelf = _dataContext.Shelf.Find(shelfId);

            if (shelf != null)
            {
                var booksList = _dataContext.Books.Where(book => books.Contains(book.BookId)).ToList();

                foreach (Book book in booksList)
                {
                    shelf.Books.Remove(book);
                }
            }

            return _dataContext.SaveChanges();
        }

        public int UpdateShelf(Shelf updatedshelf)
        {
            var shelf = _dataContext.Shelf.Find(updatedshelf.ShelfId);

            if(shelf !=  null)
            {
                shelf.Name = updatedshelf.Name;
                shelf.Description = updatedshelf.Description;
            }

            return _dataContext.SaveChanges();
        }

        public int DeleteShelf(int shelfId)
        {
            var shelf = _dataContext.Shelf.Find(shelfId);

            if(shelf != null)
            {
                _dataContext.Shelf.Remove(shelf);
            }
            
            return _dataContext.SaveChanges();
        }
    }
}
