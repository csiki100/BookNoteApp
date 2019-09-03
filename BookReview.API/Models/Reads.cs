namespace BookReview.API.Models
{
    public class Reads
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
    }
}