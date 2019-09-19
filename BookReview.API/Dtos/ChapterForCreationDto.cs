using System.ComponentModel.DataAnnotations;

namespace BookReview.API.Dtos
{
    public class ChapterForCreationDto
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string ChapterName { get; set; }
        [Required]
        public string Content { get; set; }
    }
}