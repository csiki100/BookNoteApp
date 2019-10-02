using System.ComponentModel.DataAnnotations;

namespace BookReview.API.Dtos
{
    ///<summary>
    /// Data Transfer Object that carries the data of a User who wants to Log in
    /// Carries data from the Client to the Server.
    ///</summary>
    public class UserForLoginDto
    {
        ///<summary>
        /// Property that contains the Username of the User
        ///</summary>
        [Required]
        public string Username { get; set; }

        ///<summary>
        /// Property that contains the Password of the User
        ///</summary>
        [Required]
        public string Password { get; set; }
    }
}