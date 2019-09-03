using System.Threading.Tasks;
using BookReview.API.Dtos;
using BookReview.API.Helpers;
using BookReview.API.Models;

namespace BookReview.API.Data
{
    public interface IBookRepository
    {
        Task<PagedList<Book>> GetBooksForUser(int userId, int pageNumber, int pageSize);
        Task<Book> GetBook(int bookId);
        Task<User> GetUser(int userId);
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
    }
}