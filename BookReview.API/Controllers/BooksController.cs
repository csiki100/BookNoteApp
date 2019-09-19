using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookReview.API.Data;
using BookReview.API.Dtos;
using BookReview.API.Helpers;
using BookReview.API.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookReview.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public BooksController(IBookRepository repo, IMapper mapper,
        IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);

        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBooksForUser(int userId)
        {

            var books = await _repo.GetBooksForUser(userId);

            var booksToReturn = _mapper.Map<IEnumerable<BookForUserListDto>>(books);

            return Ok(booksToReturn);
        }


        [HttpGet("book/{bookId}", Name = "GetBookDetail")]
        public async Task<IActionResult> GetBookDetail(int bookId)
        {
            var book = await _repo.GetBook(bookId);

            if (book == null)
                return NotFound();

            var bookToReturn = _mapper.Map<BookForDetailedDto>(book);

            return Ok(bookToReturn);
        }

        [HttpDelete("{userId}/{bookId}")]
        public async Task<IActionResult> DeleteBook(int userId, int bookId)
        {
            //Checking if userId matches the current user's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the user has got the book in his/her library
            var read = await _repo.GetRead(userId, bookId);

            //if they don't we return Unathorized
            if(read==null){
                return Unauthorized();
            }
            //if they do, we delete the book from the library
            _repo.Delete(read);

            var book = await _repo.GetBook(bookId);

            if(book!=null){
                //if the book is not in any of the other libraries, we delete the book as well
                if(book.UsersWhoRead.Count==1){
                    _repo.Delete(book);
                }
                //Saving changes
                if (await _repo.SaveAll())
                {
                    return NoContent();
                }
            }
            return BadRequest("Could not delete book");
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddBook(int userId,
        [FromForm]BookForCreationDto bookForCreationDto)
        {
            //Checking if the userId matches the currently logged in user's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var file = bookForCreationDto.File;
            var book = _mapper.Map<Book>(bookForCreationDto);

            //uploading the book's picture to the cloud
            var result=_repo.UploadPictureToCloud(book, file);

            //if successful
            if (result)
            {
                var userFromRepo = await _repo.GetUser(userId);
                if (userFromRepo!=null)
                {
                    //Adding the book to the user's library and to the database
                    var read = new Read();
                    read.Book = book;
                    read.User = userFromRepo;

                    _repo.Add(book);
                    _repo.Add(read);

                    //Saving changes
                    if (await _repo.SaveAll())
                    {
                        var bookToReturn = _mapper.Map<BookForDetailedDto>(book);
                        return CreatedAtRoute("GetBookDetail", new { bookId = book.Id }, bookToReturn);
                    }
                }
            }
            return BadRequest("Could not add the book");
        }

        [HttpPost("{userId}/{bookId}")]
        public async Task<IActionResult> EditBook(int userId,int bookId, [FromForm]BookForEditDto BookForEditDto)
        {
            //Checking if the userId matches the currently logged in user's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the book is in the user's library
            if(await _repo.GetRead(userId,bookId)==null){
                return BadRequest();
            }

            var book = await _repo.GetBook(bookId);

            //if the user wants to change the book's title
            if(BookForEditDto.Name!=null){
                book.Name = BookForEditDto.Name;
            }

            //if the user wants to change the book's picture
            if (BookForEditDto.File != null)
            {
                var prevPublicId = book.Picture.PublicId;

                //uploading the picture to the cloud
                var response = _repo.UploadPictureToCloud(book, BookForEditDto.File);

                //if everything went well
                if (response)
                {
                    //we delete the previous picture from cloud
                    _repo.DeletePictureFromCloud(prevPublicId);
                }
            }

            //Saving changes
            if (await _repo.SaveAll())
            {
                var bookToReturn = _mapper.Map<BookForDetailedDto>(book);
                return Ok(bookToReturn);
            }

            return BadRequest("Could not edit the book");
        }
    }
}