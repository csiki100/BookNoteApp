using BookReview.API.Models;

namespace BookReview.API.Dtos
{
    public class BookForCreationDto
    {
        public string Name { get; set; }
        public Picture Picture { get; set; }
    }
}