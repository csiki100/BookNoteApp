using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookReview.API.Data;
using BookReview.API.Dtos;
using BookReview.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBooksForUser(int userId,BodyParams bodyParams)
        {

            var books = await _repo.GetBooksForUser(userId,
             bodyParams.PageNumber, bodyParams.PageSize);
            var booksToReturn = _mapper.Map<IEnumerable<BookForUserListDto>>(books);

            return Ok(booksToReturn);
        }


        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetBookDetail(int bookId){
            var book = await _repo.GetBook(bookId);

            if(book==null)
                return NotFound();

            var bookToReturn = _mapper.Map<BookForDetailedDto>(book);

            return Ok(bookToReturn);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId){

            var book = await _repo.GetBook(bookId);

            _repo.Delete(book);

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("Error while deleting message");
        }

        // [HttpPost]
        // public async Task<IActionResult> AddBook([FromForm]BookForCreationDto bookForCreationDto){
            
        // }

    }
}