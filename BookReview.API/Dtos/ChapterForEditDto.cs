using System.ComponentModel.DataAnnotations;

namespace BookReview.API.Dtos
{
    public class ChapterForEditDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BookId { get; set; }
        public string ChapterName { get; set; }
        public string Content { get; set; }
        
    }
}