using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookReview.API.Dtos
{
    ///<summary>
    /// Data Transfer Object that carries the editing data of a Book
    /// Carries data from the Client to the Server.
    ///</summary>
    public class BookForEditDto
    {
        ///<summary>
        /// Property that contains the new Name of the Book
        ///</summary>
        public string Name { get; set; }
        ///<summary>
        /// Property that contains the new Picture of the Book
        ///</summary>
        public IFormFile File { get; set; }
    }
}