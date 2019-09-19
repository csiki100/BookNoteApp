using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookReview.API.Dtos;
using BookReview.API.Helpers;
using BookReview.API.Models;
using Microsoft.AspNetCore.Http;

namespace BookReview.API.Data
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksForUser(int userId);
        Task<Book> GetBook(int bookId);
        Task<User> GetUser(int userId);
        Task<Chapter> GetChapter(int userId, int bookId, int chapterId);
        Task<ICollection<Chapter>> GetChaptersForBook(int userId, int bookId);
        Task<Read> GetRead(int userId, int bookId);
        Boolean UploadPictureToCloud(Book book, IFormFile picture);
        void DeletePictureFromCloud(string publicID);

        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
    }
}