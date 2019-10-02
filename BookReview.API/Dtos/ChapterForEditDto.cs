using System.ComponentModel.DataAnnotations;

namespace BookReview.API.Dtos
{

    ///<summary>
    /// Data Transfer Object that carries the editing data of a Chapter.
    /// Carries data from the Client to the Server.
    ///</summary>
    public class ChapterForEditDto
    {

        ///<summary>
        /// Property that contains the Id of the Chapter
        ///</summary>
        [Required]
        [Range(1,int.MaxValue)]
        public int Id { get; set; }

        ///<summary>
        /// Property that contains the Id of the User
        ///</summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        ///<summary>
        /// Property that contains the Id of the Book
        ///</summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int BookId { get; set; }

        ///<summary>
        /// Property that contains the new Name of the Chapter
        ///</summary>
        public string ChapterName { get; set; }

        ///<summary>
        /// Property that contains the new Content of the Chapter
        ///</summary>
        public string Content { get; set; }
        
    }
}