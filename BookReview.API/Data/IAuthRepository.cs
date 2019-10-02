using System.Threading.Tasks;
using BookReview.API.Models;

namespace BookReview.API.Data
{
    ///<summary>
    /// Interface that contains all the functions an Authentication class needs to implement
    ///</summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Registers a new user, and saves it's data to the database
        ///</summary> 
        /// <param name="user">
        /// Object that contains the user's data
        ///</param>
        /// <param name="password">
        /// string that contains the user's password
        ///</param>
        ///<returns>
        /// Returns the user parameter, with the set password fields
        ///</returns>
        Task<User> Register(User user, string password);


        /// <summary>
        /// Asynchronous method, that checks if a user exists
        ///</summary> 
        /// <param name="username">
        /// string that represents the user the function searches for
        ///</param>
        ///<returns>
        /// returns:
        /// true if the user is found,
        /// false if not
        ///</returns>
        Task<bool> UserExists(string username);


        /// <summary>
        /// Function that checks a user's login credentials
        ///</summary> 
        /// <param name="username">
        /// string that contains the user's username
        ///</param>
        /// <param name="password">
        /// string that contains the user's password
        ///<returns>
        /// returns:
        /// the user, if the credentials are correct
        /// null, if not
        ///</returns>
        Task<User> Login(string username, string password);
    }
}