using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookReview.API.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookReview.API.Data
{
    public class BookRepository : IBookRepository
    {
        ///<summary>
        /// Variable, the application can communicate with the database through this
        ///</summary>
        private readonly DataContext _context;

        /// <summary>
        /// Cloudinary that is used for storing images in the cloud
        /// </summary>
        private Cloudinary _cloudinary;

        /// <summary>
        /// Configuration of Cloudinary
        /// </summary>
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        /// <summary>
        /// Class Constructor
        /// </summary>
        public BookRepository(DataContext context,
        IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _context = context;

            //Initializing Cloudinary
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }


        /// <summary>
        /// Adds an entity to the database
        ///</summary> 
        /// <param name="entity">
        /// Entity that will be added to the database
        ///</param>
        ///<returns>
        /// void
        ///</returns>
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }


        /// <summary>
        /// Deletes an entity from the database
        ///</summary> 
        /// <param name="entity">
        /// Entity that will deleted
        ///</param>
        ///<returns>
        /// void
        ///</returns>
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }


        /// <summary>
        /// Asynchronous function that retrieves a book from the database
        ///</summary> 
        /// <param name="userId">
        /// Integer that contains the user's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that contains the book's ID
        ///</param>
        ///<returns>
        /// Returns:
        /// The book if it was found,
        /// null if not
        ///</returns>
        public async Task<Book> GetBook(int userId, int bookId)
        {
            //Getting the book from the database
            var book= await _context.Books
            .Include(b => b.Picture)
            .Include(b => b.Chapters)
            .Include(b => b.UsersWhoRead)
            .FirstOrDefaultAsync(b => b.Id == bookId);


            //Checking if a book was found
            if(book==null){
                return null;
            }

            //Checking if the user has the book in his/her library
            if (book.UsersWhoRead.Any(r => r.UserId == userId)){
                return book;
            }
            else{
                return null;
            }
            
        }



        /// <summary>
        /// Asynchronous function that retrieves the books of a user from the database
        ///</summary> 
        /// <param name="userId">
        /// Integer that contains the user's ID
        ///</param>
        ///<returns>
        /// Returns:
        /// The books of the user as a List
        ///</returns>
        public async Task<List<Book>> GetBooksForUser(int userId)
        {
            //getting the book ids
            var bookIds = _context.Reads.Where(r => r.UserId == userId).Select(r => r.BookId);

            //getting the books
            var books = await _context.Books.Include(b => b.Picture)
            .Where(b => bookIds.Contains(b.Id)).OrderBy(b => b.Name).ToListAsync();

            return books;
        }



        /// <summary>
        /// Asynchronous function that retrieves a user from the database
        ///</summary> 
        /// <param name="userId">
        /// Integer that contains the user's ID
        ///</param>
        ///<returns>
        /// Returns:
        /// The user
        ///</returns>
        public async Task<User> GetUser(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }



        /// <summary>
        /// Asynchronous function that retrieves a chapter from the database
        ///</summary> 
        /// <param name="userId">
        /// Integer that contains the user's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that contains the book's ID
        ///</param>
        /// <param name="chapterId">
        /// Integer that contains the chapter's ID
        ///</param>
        ///<returns>
        /// Returns:
        /// The chapter
        ///</returns>
        public async Task<Chapter> GetChapter(int userId, int bookId, int chapterId)
        {
            return await _context.Chapters.FirstOrDefaultAsync
            (c => c.Id == chapterId && c.BookId == bookId && c.UserId == userId);
        }



        /// <summary>
        /// Asynchronous function that saves the changes in the database
        ///</summary> 
        ///<returns>
        /// Returns:
        /// true, if there were any changes saved
        /// false, if not
        ///</returns>
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }



        /// <summary>
        /// function that uploads a picture to the cloud
        /// and writes the picture data into the Book entity
        ///</summary> 
        /// <param name="book">
        /// Entity, the picture will be bound to this
        ///</param>
        /// <param name="picture">
        /// Object that represents the picture
        ///</param>
        ///<returns>
        /// void
        ///</returns>
        public void UploadPictureToCloud(Book book, IFormFile picture)
        {
            //checking picture size
            if (picture.Length == 0)
            {
                throw new Exception("Encountered an error while uploading the picture.");
            }

            //checking image format
            if(picture.ContentType != "image/jpeg" && picture.ContentType != "image/png"){

                throw new Exception("Bad Image Format");
            }

            using (var stream = picture.OpenReadStream())
            {
                //setting up upload parameters
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(picture.Name, stream),
                    Transformation = new Transformation()
                        .Width(500).Crop("scale")
                };

                //uploading the image
                var uploadResult = _cloudinary.Upload(uploadParams);

                //checking if there were and errors
                if (uploadResult.Error != null)
                {
                    throw new Exception(
                        "Encountered an error while uploading the picture.");
                }


                //Initializing Picture Entity
                var pictureForCreation = new Picture();
                pictureForCreation.Url = uploadResult.Uri.ToString();
                pictureForCreation.PublicId = uploadResult.PublicId;
                pictureForCreation.Book = book;
                pictureForCreation.BookId = book.Id;

                //Adding the Picture to the database
                _context.Add(pictureForCreation);
            }
        }


        /// <summary>
        /// function that deletes a picture from the cloud
        ///</summary> 
        /// <param name="publicID">
        /// string that contains the Picture's Public ID
        ///</param>
        ///<returns>
        /// void
        ///</returns>
        public void DeletePictureFromCloud(string publicID)
        {
            var result = _cloudinary.DeleteResources(publicID);
            if (result.Error != null)
            {
                throw new Exception("Encountered an error while processing the request");
            }
        }


        /// <summary>
        /// Asynchronous function, that retrieves a Read Entity from the database
        ///</summary> 
        /// <param name="userId">
        /// Integer that contains the User's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that contains the Book's ID
        ///</param>
        ///<returns>
        /// Returns:
        /// The Read entity
        ///</returns>
        public async Task<Read> GetRead(int userId, int bookId)
        {
            return await _context.Reads.FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId);

        }


        /// <summary>
        /// Asynchronous function, that retrieves a Book's chapters from the database
        ///</summary> 
        /// <param name="userId">
        /// Integer that contains the User's ID
        ///</param>
        /// <param name="bookId">
        /// Integer that contains the Book's ID
        ///</param>
        ///<returns>
        /// Returns:
        /// The Chapters as a List
        ///</returns>
        public async Task<ICollection<Chapter>> GetChaptersForBook(int userId, int bookId)
        {
            return await _context.Chapters.Where(c => c.BookId == bookId && c.UserId == userId).ToListAsync();

        }
    }


}