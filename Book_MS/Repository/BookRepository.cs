using Book_MS.Data;
using Book_MS.Models;

namespace Book_MS.Repository
{
    public class BookRepository
    {
        // Set private property to acces the DataContext
        private readonly DataContext _dataContext;

        public BookRepository(DataContext context)
        {
            _dataContext = context;
        }

        public List<Book> GetBooks()
        {
            return _dataContext.Books.ToList();
        }

        public Book? GetBookById(int bookId)
        {
            return _dataContext.Books.Find(bookId);
        }

        // Get all the books by given author ID
        public List<Book> GetBooksByAuthor(int authorId)
        {
            int numberOfAuthor = _dataContext.Authors.Where(author => author.Books.Count > 0).Count();

            return _dataContext.Books.Where(bk => bk.AuthorId == authorId).ToList();
        }

        public int AddBookAward(int bookId, int awardId)
        {
            var book = _dataContext.Books.Find(bookId);

            if(book != null)
            {
                var award = _dataContext.Awards.Find(awardId);

                if(award != null)
                {
                    book.Awards.Add(award);
                }

                if(award != null)
                {
                    return 0;
                }
            }

            return _dataContext.SaveChanges();
        }

        public int RemoveBookAward(int bookId, int awardId)
        {
            var book = _dataContext.Books.Find(bookId);

            if (book != null)
            {
                var award= _dataContext.Awards.Find(awardId);

                if (award != null)
                {
                    book.Awards.Remove(award);
                }

                if (award != null)
                {
                    return 0;
                }
            }

            return _dataContext.SaveChanges();
        }

        public int AddBookGenre(int bookId, List<int> genreIds)
        {
            var book = _dataContext.Books.Find(bookId);

            if (book != null)
            {
                var genreList = _dataContext.Genres.Where(gr => genreIds.Contains(gr.GenreId)).ToList();

                foreach (Genre genre in genreList)
                {
                    book.Genres.Add(genre);
                }
            }

            return _dataContext.SaveChanges();
        }

        public int RemoveBookGenre(int bookId, List<int> genreIds)
        {
            var book = _dataContext.Books.Find(bookId);

            if (book != null)
            {
                var genreList = _dataContext.Genres.Where(gr => genreIds.Contains(gr.GenreId)).ToList();

                foreach (Genre genre in genreList)
                {
                    book.Genres.Remove(genre);
                }
            }

            return _dataContext.SaveChanges();
        }

        // Add a book to the database
        public int CreateBook(Book book)
        {
            _dataContext.Books.Add(book);

            // Will return the number of affected rows
            return _dataContext.SaveChanges();
        }

        public int CountBooks(string title)
        {
           return _dataContext.Books.Where(bk =>bk.Title == title).Count();
        }

        // Delete a book from the database
        public int DeleteBook(int Bookid)
        {
            // Find the book to delete. The 'FIND' method always use the primary key to search the object
            var book = _dataContext.Books.Find(Bookid);

            if(book != null)
            {
                book.IsActive = 0;
            }
            
            // Will return the number of affected rows
            return _dataContext.SaveChanges();
        }

        public int UpdateBook(Book updatedBook)
        {
            var book = _dataContext.Books.Find(updatedBook.BookId);

            if(book != null)
            {
                book.Title = updatedBook.Title;
                book.Year = updatedBook.Year;
                book.AuthorId = updatedBook.AuthorId;
            }

            return _dataContext.SaveChanges();
        }
    }
}
