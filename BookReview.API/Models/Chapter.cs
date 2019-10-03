using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReview.API.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public virtual Book Book { get; set; }
        public int BookId { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public string ChapterName { get; set; }
        public string Content { get; set; }
    }
}