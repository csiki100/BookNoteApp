using System.Linq;
using System.Threading.Tasks;
using BookReview.API.Dtos;
using BookReview.API.Helpers;
using BookReview.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReview.API.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;
        public BookRepository(DataContext context )
        {
            _context = context;

        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<Book> GetBook(int bookId)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<PagedList<Book>> GetBooksForUser(int userId, int pageNumber, int pageSize)
        {
            var user = await _context.Users.Include(u=>u.Books).FirstOrDefaultAsync(u => u.Id == userId);

            var bookIds = user.Books
            .Where(r => r.UserId == userId)
            .Select(r => r.BookId);

            var books = _context.Books.Where(b => bookIds.Contains(b.Id));

            return await PagedList<Book>.CreateAsync(books, pageNumber, pageSize);
        }

        public Task<User> GetUser(int userId)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        
    }
}