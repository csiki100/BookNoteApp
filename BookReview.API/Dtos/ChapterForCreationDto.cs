using System.ComponentModel.DataAnnotations;

namespace BookReview.API.Dtos
{
    ///<summary>
    /// Data Transfer Object that carries the data of a Chapter to be created
    /// Carries data from the Client to the Server.
    ///</summary>
    public class ChapterForCreationDto
    {
        ///<summary>
        /// Property that contains the Id of the Book
        ///</summary>
        [Required]
        [Range(1,int.MaxValue)]
        public int BookId { get; set; }

        ///<summary>
        /// Property that contains the Id of the User
        ///</summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        ///<summary>
        /// Property that contains the Name of the Chapter
        ///</summary>
        [Required]
        public string ChapterName { get; set; }

        ///<summary>
        /// Property that contains the Content of the Chapter
        ///</summary>
        [Required]
        public string Content { get; set; }
    }
}