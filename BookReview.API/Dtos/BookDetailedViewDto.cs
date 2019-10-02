using System.Collections.Generic;
using BookReview.API.Models;

namespace BookReview.API.Dtos
{
    ///<summary>
    /// Data Transfer Object that carries the data of a Book
    /// Carries data from the Server to the Client. Used for a detailed view.
    ///</summary>
    public class BookDetailedViewDto
    {
        ///<summary>
        /// Property that contains the Id of the Book
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// Property that contains the Name of the Book
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// Property that contains the Chapters of the Book
        ///</summary>
        public ICollection<ChapterDetailedViewDto> Chapters { get; set; }

        ///<summary>
        /// Property that contains the Picture of the Book
        ///</summary>
        public PictureDto Picture { get; set; }
    }
}