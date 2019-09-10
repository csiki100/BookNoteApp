using System.IO;
using BookReview.API.Models;
using Microsoft.AspNetCore.Http;

namespace BookReview.API.Dtos
{
    public class BookForCreationDto
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}