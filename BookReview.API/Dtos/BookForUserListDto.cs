using BookReview.API.Models;

namespace BookReview.API.Dtos
{
    public class BookForUserListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Picture Picture { get; set; }
    }
}