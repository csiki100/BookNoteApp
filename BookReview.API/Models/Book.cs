using System.Collections.Generic;

namespace BookReview.API.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Chapter> Chapters { get; set; }
        public Picture Picture { get; set; }
        public ICollection<Read> UsersWhoRead { get; set; }
        
    }
}