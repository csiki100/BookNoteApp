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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;
        public ChaptersController(IBookRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        [HttpPost]
        public async Task<IActionResult> AddChapter(ChapterForCreationDto chapterForCreationDto)
        {

            //Checking if the userId matches the currently logged in user's ID
            if (chapterForCreationDto.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the book is in the user's library
            if (await _repo.GetRead(chapterForCreationDto.UserId, chapterForCreationDto.BookId) == null)
            {
                return Unauthorized();
            }

            //Initializing the Chapter the User would like to add to the database
            var book = await _repo.GetBook(chapterForCreationDto.BookId);
            var user = await _repo.GetUser(chapterForCreationDto.UserId);

            var chapter = _mapper.Map<Chapter>(chapterForCreationDto);
            chapter.Book = book;
            chapter.User = user;

            //Adding to Chapter to the database
            _repo.Add(chapter);

            //Saving changes
            if(await _repo.SaveAll()){
                var chapterToReturn = _mapper.Map<ChapterToReturnDto>(chapter);
                return CreatedAtRoute("GetChapter",
                new {userId=user.Id,bookId=book.Id, chapterId=chapter.Id},
                 chapterToReturn);
            }
            return BadRequest("Could not add chapter");
        }

        [HttpGet("{userId}/{bookId}/{chapterId}",Name="GetChapter")]
        public async Task<IActionResult> GetChapter(int userId,int bookId, int chapterId){

            //Checking if the userId matches the currently logged in user's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the book is in the user's library
            if (await _repo.GetRead(userId, bookId) == null)
            {
                return Unauthorized();
            }

            //Getting the chapter from the database
           var chapter = await _repo.GetChapter(userId, bookId, chapterId);

           if(chapter==null){
                return BadRequest();
            }

            var chapterToReturn = _mapper.Map<ChapterToReturnDto>(chapter);
            return Ok(chapterToReturn);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditChapter(ChapterForEditDto chapterForEdit){
            

            //Checking if the userId matches the currently logged in user's ID
            if (chapterForEdit.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            
            //Checking if the book is in the user's library
            if (await _repo.GetRead(chapterForEdit.UserId,chapterForEdit.BookId) == null)
            {
                return Unauthorized();
            }

            //Getting the chapter from the database
            var chapter = await _repo.GetChapter(
                chapterForEdit.UserId, chapterForEdit.BookId, chapterForEdit.Id);

            //checking if the chapter exists
            if (chapter == null)
            {
                return BadRequest();
            }

            //Updating the values
            if(chapterForEdit.ChapterName!=null){
                chapter.ChapterName = chapterForEdit.ChapterName;
            }
            if(chapterForEdit.Content!=null){
                chapter.Content = chapterForEdit.Content;
            }
            
            //Saving changes
            if(await _repo.SaveAll()){
                return Ok();
            }
            return BadRequest("Could not edit chapter");
        }

        [HttpDelete("{userId}/{bookId}/{chapterId}")]
        public async Task<IActionResult> DeleteChapter(int userId,int bookId,int chapterId){

            //Checking if the userId matches the currently logged in user's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the book is in the user's library
            if(await _repo.GetRead(userId,bookId)==null){
                return Unauthorized();
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
                return Ok();
            }
            return BadRequest("Could not delete chapter");
        }

        [HttpGet("{userId}/{bookId}")]
        public async Task<IActionResult> GetChaptersForBook(int userId, int bookId){
            //Checking if the userId matches the currently logged in user's ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //Checking if the book is in the user's library
            if (await _repo.GetRead(userId, bookId) == null)
            {
                return Unauthorized();
            }

            //Getting the chapters
            var chapters = await _repo.GetChaptersForBook(userId, bookId);
            
            //Mapping them to the correct format
            var chaptersToReturn = _mapper.Map<ICollection<ChapterToReturnDto>>(chapters);

            return Ok(chaptersToReturn);
        }
    }
}