using System.Threading.Tasks;
using BookReview.API.Models;

namespace BookReview.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<bool> UserExists(string username);
    }
}