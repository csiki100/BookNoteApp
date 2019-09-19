using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookReview.API.Dtos;
using BookReview.API.Helpers;
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
        private readonly DataContext _context;
       
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public BookRepository(DataContext context,
        IOptions<CloudinarySettings> cloudinaryConfig )
        {
            _cloudinaryConfig = cloudinaryConfig;
            _context = context;


            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

    public void Add<T>(T entity) where T : class
    {
        _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
        _context.Remove(entity);
    }
    public async Task<Book> GetBook(int bookId)
    {
        return await _context.Books
        .Include(b => b.Picture)
        .Include(b => b.Chapters)
        .Include(b=>b.UsersWhoRead)
        .FirstOrDefaultAsync(b => b.Id == bookId);
    }

    public async Task<List<Book>> GetBooksForUser(int userId)
    {
        var user = await _context.Users.Include(u => u.Books).FirstOrDefaultAsync(u => u.Id == userId);

        var bookIds = user.Books
        .Where(r => r.UserId == userId)
        .Select(r => r.BookId);

        var books = _context.Books.Include(b => b.Picture).Where(b => bookIds.Contains(b.Id)).OrderBy(b=>b.Name).ToList();

            return books;
        }

    public async Task<User> GetUser(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

     public async Task<Chapter> GetChapter(int userId, int bookId, int chapterId)
    {
        return await _context.Chapters.FirstOrDefaultAsync
        (c => c.Id == chapterId && c.BookId == bookId && c.UserId == userId);
    }

    public async Task<bool> SaveAll()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public Boolean UploadPictureToCloud(Book book, IFormFile picture)
    {

        var uploadResult = new ImageUploadResult();

        if (picture.Length > 0)
        {
            using (var stream = picture.OpenReadStream())
            {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(picture.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("auto")
                };

                uploadResult = _cloudinary.Upload(uploadParams);
            }
                if (uploadResult.PublicId != null)
                {

                    var pictureForCreation = new Picture();
                    pictureForCreation.Url = uploadResult.Uri.ToString();
                    pictureForCreation.PublicId = uploadResult.PublicId;
                    pictureForCreation.Book = book;

                    _context.Add(pictureForCreation);

                    return true;
                }
            }
            return false;
    }

    public void DeletePictureFromCloud(string publicID){
            _cloudinary.DeleteResources(publicID);
        }


        public async Task<Read> GetRead(int userId, int bookId)
        {
             return await _context.Reads.FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId);

        }

        public async Task<ICollection<Chapter>> GetChaptersForBook(int userId, int bookId)
        {
            return await _context.Chapters.Where(c => c.BookId == bookId && c.UserId == userId).ToListAsync();

        }
    }


}