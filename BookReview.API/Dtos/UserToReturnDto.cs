namespace BookReview.API.Dtos
{
    ///<summary>
    /// Data Transfer Object that carries the data of a User.
    /// Carries data from the Server to the Client.
    ///</summary>
    public class UserToReturnDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}