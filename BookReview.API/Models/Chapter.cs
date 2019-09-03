namespace BookReview.API.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public int ChapterNum { get; set; }
        public string Content { get; set; }
    }
}