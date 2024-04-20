using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace webapi
{
    public static class LinqExtendsion
    {
        public static async Task<Page<TSource>> ToPageListAsync<TSource>(this IQueryable<TSource> list, PageInfo result)
        {
            var takePage = list.Skip(result.PageIndex * result.PageSize).Take(result.PageSize);
            Page<TSource> pageList = new Page<TSource>();
            pageList.PageIndex = result.PageIndex;
            pageList.PageSize = result.PageSize;
            pageList.TotalCount = list.Count();
            pageList.Data = await takePage.ToListAsync();
            return pageList;
        }

        public static Page<TSource> ToPageList<TSource>(this IQueryable<TSource> list, PageInfo result)
        {
            var takePage = list.Skip(result.PageIndex * result.PageSize).Take(result.PageSize);
            Page<TSource> pageList = new Page<TSource>();
            pageList.PageIndex = result.PageIndex;
            pageList.PageSize = result.PageSize;
            pageList.TotalCount = list.Count();
            pageList.Data = takePage.ToList();
            return pageList;
        }
    }

    public class PageInfo
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class Page<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
