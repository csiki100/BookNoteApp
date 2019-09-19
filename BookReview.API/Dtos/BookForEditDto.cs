using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookReview.API.Dtos
{
    public class BookForEditDto
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}