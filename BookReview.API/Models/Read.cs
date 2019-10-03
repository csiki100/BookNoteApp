namespace BookReview.API.Models
{
    public class Read
    {
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual Book Book { get; set; }
        public int BookId { get; set; }
    }
}