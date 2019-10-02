using System.ComponentModel.DataAnnotations;

namespace BookReview.API.Dtos
{
    ///<summary>
    /// Data Transfer Object that carries the data of a User to be registered.
    /// Carries data from the Client to the Server.
    ///</summary>
    public class UserForRegisterDto
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
        [StringLength(20,MinimumLength=4,ErrorMessage="Password has to be between 4 and 20 characters")]
        public string Password { get; set; }
    }
}