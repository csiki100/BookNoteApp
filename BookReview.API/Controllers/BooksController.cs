using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> GetBooksForUser(int userId, [FromQuery]BodyParams bodyParams)
        {

            var books = await _repo.GetBooksForUser(userId,
             bodyParams.PageNumber, bodyParams.PageSize);
            var booksToReturn = _mapper.Map<IEnumerable<BookForUserListDto>>(books);

            return Ok(booksToReturn);
        }


        [HttpGet("book/{bookId}",Name="GetBookDetail")]
        public async Task<IActionResult> GetBookDetail(int bookId)
        {
            var book = await _repo.GetBook(bookId);

            if (book == null)
                return NotFound();

            var bookToReturn = _mapper.Map<BookForDetailedDto>(book);

            return Ok(bookToReturn);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {

            var book = await _repo.GetBook(bookId);

            _repo.Delete(book);

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("Error while deleting message");
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(
        [FromForm]BookForCreationDto bookForCreationDto)
        {
            // var userFromRepo = await _repo.GetUser(userId);

            var file = bookForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            var book = _mapper.Map<Book>(bookForCreationDto);

            var pictureForCreation = new Picture();
            pictureForCreation.Url = uploadResult.Uri.ToString();
            pictureForCreation.PublicId = uploadResult.PublicId;
            pictureForCreation.Book = book;

            book.Picture = pictureForCreation;
            _repo.Add(book);

            
            if (await _repo.SaveAll())
            {
                return CreatedAtRoute("GetBookDetail", new { bookId = book.Id }, book);
            }


            return BadRequest("Could not add the book");


            
        }

    }
}