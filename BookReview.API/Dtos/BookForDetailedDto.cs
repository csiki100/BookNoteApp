using System.Collections.Generic;
using BookReview.API.Models;

namespace BookReview.API.Dtos
{
    public class BookForDetailedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Chapter> Chapters { get; set; }
        public Picture Picture { get; set; }
    }
}