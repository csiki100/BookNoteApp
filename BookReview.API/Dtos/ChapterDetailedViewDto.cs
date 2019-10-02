namespace BookReview.API.Dtos
{

    ///<summary>
    /// Data Transfer Object that carries the data of a Chapter
    /// Carries data from the Server to the Client. Used for a detailed view.
    ///</summary>
    public class ChapterDetailedViewDto
    {

        ///<summary>
        /// Property that contains the Id of the Chapter
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// Property that contains the Id of the Book
        ///</summary>
        public int BookId { get; set; }

        ///<summary>
        /// Property that contains the Id of the User
        ///</summary>
        public int UserId { get; set; }

        ///<summary>
        /// Property that contains the Name of the Chapter
        ///</summary>
        public string ChapterName { get; set; }

        ///<summary>
        /// Property that contains the Content of the Chapter
        ///</summary>
        public string Content { get; set; }
    }
}