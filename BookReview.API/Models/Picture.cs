namespace BookReview.API.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
    }
}