using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookReview.API.Data;
using BookReview.API.Dtos;
using BookReview.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.API.Controllers
{
    /// <summary>
    /// Controller that contains the methods to retrieve and edit the data of "Chapters"
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        /// <summary>
        /// Repository that contains the implementations of methods to retrieve and manipulate
        /// the data of Chapters
        /// </summary>
        private readonly IBookRepository _repo;

        /// <summary>
        /// AutoMapper that is used for object-object mapping
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Class Constructor
        /// </summary>
        public ChaptersController(IBookRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }


        /// <summary>
        /// Adds a new chapter to a Book
        ///</summary> 
        /// <param name="chapterForCreationDto">
        /// Object that contains the new chapter's data
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 201: the request was successful, returns the created chapter in the body
        /// 401: UserID(in chapterForCreationDto) didn't match the logged in User's ID
        /// 400: If the chapter couldn't be added,
        /// or the Book is not in the User's library
        ///</returns>
        [HttpPost]
        public async Task<IActionResult> AddChapter(ChapterForCreationDto chapterForCreationDto)
        {

            //Checking if the userId matches the currently logged in User's ID
            if (chapterForCreationDto.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the Book is in the User's library
            if (await _repo.GetRead(chapterForCreationDto.UserId, chapterForCreationDto.BookId) == null)
            {
                return BadRequest();
            }

            //Initializing the Chapter the User would like to add to the database
            var book = await _repo.GetBook(chapterForCreationDto.UserId,chapterForCreationDto.BookId);
            var user = await _repo.GetUser(chapterForCreationDto.UserId);

            var chapter = _mapper.Map<Chapter>(chapterForCreationDto);
            chapter.Book = book;
            chapter.User = user;

            //Adding to Chapter to the database
            _repo.Add(chapter);

            //Saving changes
            if(await _repo.SaveAll()){
                var chapterToReturn = _mapper.Map<ChapterDetailedViewDto>(chapter);
                return CreatedAtRoute("GetChapter",
                new {userId=user.Id,bookId=book.Id, chapterId=chapter.Id},
                 chapterToReturn);
            }
            return BadRequest("Could not add chapter");
        }


        /// <summary>
        /// Retrieves a chapter
        ///</summary> 
        /// <param name="userId">
        /// Integer that represents the User's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that represents the Book's ID
        ///</param>
        /// <param name="chapterId">
        /// Integer that represents the chapter's ID
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 200: the request was successful, returns the chapter in the body
        /// 401: UserID(in chapterForCreationDto) didn't match the logged in User's ID
        /// 400: If the chapter wasn't found,
        /// or the Book is not in the User's library
        ///</returns>
        [HttpGet("{userId}/{bookId}/{chapterId}",Name="GetChapter")]
        public async Task<IActionResult> GetChapter(int userId,int bookId, int chapterId){

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

            //Getting the chapter from the database
           var chapter = await _repo.GetChapter(userId, bookId, chapterId);

           if(chapter==null){
                return BadRequest();
            }

            var chapterToReturn = _mapper.Map<ChapterDetailedViewDto>(chapter);
            return Ok(chapterToReturn);
        }


        /// <summary>
        /// Edits a chapter
        ///</summary> 
        /// <param name="chapterForEditDto">
        /// Object that contains the editing data
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 200: the edit was successful
        /// 401: UserID(in chapterForCreationDto) didn't match the logged in User's ID
        /// 400: If the chapter wasn't found, or it wasn't edited,
        /// or the Book is not in the User's library
        ///</returns>
        [HttpPost("edit")]
        public async Task<IActionResult> EditChapter(ChapterForEditDto chapterForEditDto){


            //Checking if the userId matches the currently logged in User's ID
            if (chapterForEditDto.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the Book is in the User's library
            if (await _repo.GetRead(chapterForEditDto.UserId,chapterForEditDto.BookId) == null)
            {
                return BadRequest();
            }

            //Getting the chapter from the database
            var chapter = await _repo.GetChapter(
                chapterForEditDto.UserId, chapterForEditDto.BookId, chapterForEditDto.Id);

            //checking if the chapter exists
            if (chapter == null)
            {
                return BadRequest();
            }

            //Updating the values
            if(chapterForEditDto.ChapterName!=null){
                chapter.ChapterName = chapterForEditDto.ChapterName;
            }
            if(chapterForEditDto.Content!=null){
                chapter.Content = chapterForEditDto.Content;
            }
            
            //Saving changes
            if(await _repo.SaveAll()){
                return NoContent();
            }
            return BadRequest("Chapter was not edited");
        }


        /// <summary>
        /// Deletes a chapter
        ///</summary> 
        /// <param name="userId">
        /// Integer that represents the User's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that represents the Book's ID
        ///</param>
        /// <param name="chapterId">
        /// Integer that represents the chapter's ID
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 204: the delete was successful
        /// 401: UserID(in chapterForCreationDto) didn't match the logged in User's ID
        /// 400: If the chapter wasn't found, or it wasn't deleted,
        /// or the Book is not in the User's library
        ///</returns>
        [HttpDelete("{userId}/{bookId}/{chapterId}")]
        public async Task<IActionResult> DeleteChapter(int userId,int bookId,int chapterId){

            //Checking if the userId matches the currently logged in User's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the Book is in the User's library
            if(await _repo.GetRead(userId,bookId)==null){
                return BadRequest();
            }

            //Getting the chapter from the database
            var chapter = await _repo.GetChapter(userId, bookId, chapterId);

            //checking if the chapter exists
            if (chapter == null)
            {
                return BadRequest();
            }

            //Deleting chapter
            _repo.Delete(chapter);

            //Saving changes
            if(await _repo.SaveAll()){
                return NoContent();
            }
            return BadRequest("Could not delete chapter");
        }


        /// <summary>
        /// Gets all of the chapters for a Book
        ///</summary> 
        /// <param name="userId">
        /// Integer that represents the User's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that represents the Book's ID
        ///</param>
        ///<returns>
        /// Returns on of the following Http responses:
        /// 200: the request was successful, the chapters are returned in the body
        /// 401: UserID(in chapterForCreationDto) didn't match the logged in User's ID
        /// 400: The Book is not in the User's library
        ///</returns>
        [HttpGet("{userId}/{bookId}")]
        public async Task<IActionResult> GetChaptersForBook(int userId, int bookId){
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

            //Getting the chapters
            var chapters = await _repo.GetChaptersForBook(userId, bookId);
            
            //Mapping them to the correct format
            var chaptersToReturn = _mapper.Map<ICollection<ChapterDetailedViewDto>>(chapters);

            return Ok(chaptersToReturn);
        }
    }
}