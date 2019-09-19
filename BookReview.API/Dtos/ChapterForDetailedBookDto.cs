namespace BookReview.API.Dtos
{
    public class ChapterForDetailedBookDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string ChapterName { get; set; }
        public string Content { get; set; }
    }
}