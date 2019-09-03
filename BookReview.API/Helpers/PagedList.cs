using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookReview.API.Helpers
{
    public class PagedList<T> : List<T>
    {
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int ItemsPerPage { get; set; }
            public int TotalItems { get; set; }

            public PagedList(List<T> items, int count, int pageNumber, int itemsPerPage)
            {
                CurrentPage = pageNumber;
                ItemsPerPage = itemsPerPage;
                TotalItems = count;
                TotalPages = (int)Math.Ceiling(TotalItems / (double)ItemsPerPage);
                this.AddRange(items);
            }

            public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
            int pageNumber, int pageSize)
            {        
                var count = await source.CountAsync();
                var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }
    }
}