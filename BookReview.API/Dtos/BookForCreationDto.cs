using System.ComponentModel.DataAnnotations;
using System.IO;
using BookReview.API.Models;
using Microsoft.AspNetCore.Http;

namespace BookReview.API.Dtos
{
    ///<summary>
    /// Data Transfer Object that carries the data of a new Book.
    /// Carries data from the Client to the Server.
    ///</summary>
    public class BookForCreationDto
    {
        ///<summary>
        /// Property that contains the Name of the Book
        ///</summary>
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Name { get; set; }


        ///<summary>
        /// Property that contains the Picture of the Book
        ///</summary>
        [Required]
        public IFormFile File { get; set; }
    }
}