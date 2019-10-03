using System.Collections.Generic;

namespace BookReview.API.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual ICollection<Read> UsersWhoRead { get; set; }
        
    }
}