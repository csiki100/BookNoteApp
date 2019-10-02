namespace BookReview.API.Dtos
{
    ///<summary>
    /// Data Transfer Object that carries the data of a Picture
    /// General purpose Dto
    ///</summary>
    public class PictureDto
    {
        ///<summary>
        /// Property that contains the Id of the Picture
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// Property that contains the Id of the Picture in the cloud
        ///</summary>
        public string PublicId { get; set; }

        ///<summary>
        /// Property that contains the Url of the Picture
        ///</summary>
        public string Url { get; set; }
    }
}