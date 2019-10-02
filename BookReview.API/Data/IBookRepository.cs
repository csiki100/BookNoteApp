using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookReview.API.Models;
using Microsoft.AspNetCore.Http;

namespace BookReview.API.Data
{
    ///<summary>
    /// Interface that contains all the functions 
    /// that need to be implemented for Book data manipulation
    ///</summary>
    public interface IBookRepository
    {

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
        Task<List<Book>> GetBooksForUser(int userId);

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
        Task<Book> GetBook(int userId, int bookId);

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
        Task<User> GetUser(int userId);


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
        Task<Chapter> GetChapter(int userId, int bookId, int chapterId);



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
        Task<ICollection<Chapter>> GetChaptersForBook(int userId, int bookId);



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
        Task<Read> GetRead(int userId, int bookId);


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
        void UploadPictureToCloud(Book book, IFormFile picture);


        /// <summary>
        /// function that deletes a picture from the cloud
        ///</summary> 
        /// <param name="publicID">
        /// string that contains the Picture's Public ID
        ///</param>
        ///<returns>
        /// void
        ///</returns>
        void DeletePictureFromCloud(string publicID);


        /// <summary>
        /// Adds an entity to the database
        ///</summary> 
        /// <param name="entity">
        /// Entity that will be added to the database
        ///</param>
        ///<returns>
        /// void
        ///</returns>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Deletes an entity from the database
        ///</summary> 
        /// <param name="entity">
        /// Entity that will deleted
        ///</param>
        ///<returns>
        /// void
        ///</returns>
        void Delete<T>(T entity) where T : class;


        /// <summary>
        /// Asynchronous function that saves the changes in the database
        ///</summary> 
        ///<returns>
        /// Returns:
        /// true, if there were any changes saved
        /// false, if not
        ///</returns>
        Task<bool> SaveAll();
    }
}