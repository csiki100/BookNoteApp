using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookReview.API.Data;
using BookReview.API.Dtos;
using BookReview.API.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookReview.API.Controllers
{
    /// <summary>
    /// Controller that contains the methods to retrieve and edit the data of Books
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        /// <summary>
        /// Repository that contains the implementations of methods to retrieve and manipulate
        /// the data of Books
        /// </summary>
        private readonly IBookRepository _repo;

        /// <summary>
        /// AutoMapper that is used for object-object mapping
        /// </summary>
        private readonly IMapper _mapper;

        

        /// <summary>
        /// Class Constructor
        /// </summary>
        public BooksController(IBookRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint that retrieves all of the Books of a User
        ///</summary> 
        /// <param name="userId">
        /// Integer that represents the User's ID
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 200: the request was successful, Books are in the body
        /// 401: userID didn't match the logged in User's ID
        ///</returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBooksForUser(int userId)
        {
            //Checking if userId matches the logged in User's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Getting the Books from the database
            var books = await _repo.GetBooksForUser(userId);

            //Converting the books to the proper format and returning to them to the client
            var booksToReturn = _mapper.Map<IEnumerable<BookSummarizedViewDto>>(books);

            return Ok(booksToReturn);
        }

        /// <summary>
        /// Endpoint that retrieves the data of a certain Book
        ///</summary> 
        /// <param name="userId">
        /// Integer that represents the User's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that represents the Book's ID
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 200: the request was successful, Book is in the body
        /// 401: userID didn't match the logged in User's ID
        /// 404: Book wasn't found
        ///</returns>
        [HttpGet("{userId}/{bookId}", Name = "GetBookDetail")]
        public async Task<IActionResult> GetBookDetail(int userId, int bookId)
        {
            //Checking if userId matches the logged in user's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Getting the Book from the database
            var book = await _repo.GetBook(userId, bookId);

            //Response if nothing was found 
            if (book == null)
                return NotFound();

            //Converting the Book to the proper format and returning it to the client
            var bookToReturn = _mapper.Map<BookDetailedViewDto>(book);
            
            return Ok(bookToReturn);
        }


        /// <summary>
        /// Endpoint that deletes on of the Books
        ///</summary> 
        /// <param name="userId">
        /// Integer that represents the User's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that represents the Book's ID
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 204: Delete was successful
        /// 401: userID didn't match the logged in User's ID
        /// 400: Book wasn't found, or the Book was not in the User's library
        ///</returns>
        [HttpDelete("{userId}/{bookId}")]
        public async Task<IActionResult> DeleteBook(int userId, int bookId)
        {
            //Checking if userId matches the current User's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the User has got the Book in his/her library
            var read = await _repo.GetRead(userId, bookId);

            //if not, returning Unathorized
            if (read == null)
            {
                return BadRequest();
            }
            
            // deleting the Book from the User's library
            _repo.Delete(read);

            var book = await _repo.GetBook(userId, bookId);

            if (book != null)
            {
                //if the Book is not in any of the other libraries, we delete the Book as well
                if (book.UsersWhoRead.Count == 1)
                {
                    _repo.Delete(book);
                    _repo.DeletePictureFromCloud(book.Picture.PublicId);
                }
                //Saving changes
                if (await _repo.SaveAll())
                {
                    return NoContent();
                }
            }
            //if the Book was not found
            return BadRequest("Could not delete book");
        }


        /// <summary>
        /// Endpoint that adds a new Book into a User's library
        ///</summary> 
        /// <param name="userId">
        /// Integer that represents the User's ID
        ///</param>
        /// <param name="bookForCreationDto">
        /// Object that contains the Book's data
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 201: the request was successful, returns the created Book in the body
        /// 401: userID didn't match the logged in User's ID
        /// 400: User wasn't found
        ///</returns>
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddBook(int userId,
        [FromForm]BookForCreationDto bookForCreationDto)
        {
            //Checking if the userId matches the currently logged in User's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetUser(userId);
            if (userFromRepo != null)
            {
                //Adding the Book to the User's library and to the database
                var book = _mapper.Map<Book>(bookForCreationDto);
                var read = new Read();
                read.Book = book;
                read.User = userFromRepo;

                _repo.Add(book);
                _repo.Add(read);
                //Uploading the Book's picture to the cloud
                _repo.UploadPictureToCloud(book, bookForCreationDto.File);

                //Saving changes
                if (await _repo.SaveAll())
                {
                    var bookToReturn = _mapper.Map<BookDetailedViewDto>(book);
                    return CreatedAtRoute("GetBookDetail", new { bookId = book.Id }, bookToReturn);
                }
            }
            //if the User was not found
            return BadRequest("Could not add the book");
        }


        /// <summary>
        /// Endpoint that edits a Book
        ///</summary> 
        /// <param name="userId">
        /// Integer that represents the User's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that represents the Book's ID
        ///</param>
        /// <param name="BookForEditDto">
        /// Object that contains the editing data
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 200: Everything went well, returns the edited Book in the body
        /// 401: userID didn't match the logged in User's ID
        /// 400: Book wasn't in the User's library, or there wasn't any change
        ///</returns>
        [HttpPost("{userId}/{bookId}")]
        public async Task<IActionResult> EditBook(int userId, int bookId, [FromForm]BookForEditDto BookForEditDto)
        {
            //Checking if the userId matches the currently logged in User's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the Book is in the User's library
            if (await _repo.GetRead(userId, bookId) == null)
            {
                return BadRequest();
            }

            var book = await _repo.GetBook(userId, bookId);

            //if the User wants to change the Book's title
            if (BookForEditDto.Name != null)
            {
                book.Name = BookForEditDto.Name;
            }

            //if the User wants to change the Book's picture
            if (BookForEditDto.File != null)
            {
                var prevPublicId = book.Picture.PublicId;

                //uploading the picture to the cloud
                _repo.UploadPictureToCloud(book, BookForEditDto.File);

                //deleting the previous picture from cloud
                _repo.DeletePictureFromCloud(prevPublicId);
            }

            //Saving changes
            if (await _repo.SaveAll())
            {
                var bookToReturn = _mapper.Map<BookDetailedViewDto>(book);
                return Ok(bookToReturn);
            }
            //if there were no changes
            return BadRequest("The Book was not edited.");
        }
    }
}