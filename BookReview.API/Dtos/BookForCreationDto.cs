using System.ComponentModel.DataAnnotations;
using System.IO;
using BookReview.API.Models;
using Microsoft.AspNetCore.Http;

namespace BookReview.API.Dtos
{
    public class BookForCreationDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}