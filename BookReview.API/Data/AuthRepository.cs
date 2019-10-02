using System.Threading.Tasks;
using BookReview.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReview.API.Data
{
    ///<summary>
    /// Class that contains the implementation of user authentication functions
    ///</summary>
    public class AuthRepository : IAuthRepository
    {
        ///<summary>
        /// Variable, the application can communicate with the database through this
        ///</summary>
        private readonly DataContext _context;

        ///<summary>
        /// Class Constructor
        ///</summary>
        public AuthRepository(DataContext context)
        {
            _context = context;

        }


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
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            //Creating password hash and password salt
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //Saving the user to the database
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Creates password hash and password salt for password string
        ///</summary> 
        /// <param name="password">
        /// string that contains the password
        ///</param>
        /// <param name="passwordHash">
        /// Reference where the function will copy the password hash to
        ///</param>
        /// <param name="passwordSalt">
        /// Reference where the function will copy the password salt to
        ///</param>
        ///<returns>
        /// void
        ///</returns>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }


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
        public async Task<bool> UserExists(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                return true;
            }
            return false;
        }

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
        public async Task<User> Login(string username, string password)
        {
            username = username.ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }


        /// <summary>
        /// Function that checks if a password matches a certain password hash
        ///</summary> 
        /// <param name="password">
        /// string that contains the password that needs to be checked
        ///</param>
        /// <param name="passwordHash">
        /// byte array that the password is compared to
        ///<returns>
        /// <param name="passwordSalt">
        /// byte array that is used to create a hash from the password input
        ///<returns>
        /// returns:
        /// true, if the password and the hash match
        /// false, if not
        ///</returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }
    }
}