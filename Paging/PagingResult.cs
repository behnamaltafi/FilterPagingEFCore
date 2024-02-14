using Microsoft.EntityFrameworkCore;
namespace FilterPagingEfCore.Paging
{

    public class PagingResult<T>
    {


        public PagingResult()
        {

        }

        public PagingResult(int totalCount, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public PagingResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Items.AddRange(items);
        }

        public int PageNumber { get; set; }


        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public List<T> Items { get; private set; } = new List<T>();

        public static async Task<PagingResult<T>> CreateAsync(IQueryable<T> data, int pageNumber, int pageSize)
        {
            IQueryable<T> finalData;
            var totalCount = data.Count();
            if (pageSize == -1)
            {
                finalData = data;
                var pagingResultFull = new PagingResult<T>(totalCount, pageNumber, pageSize);
                pagingResultFull.Items.AddRange(await finalData.ToListAsync());
                return pagingResultFull;
            }

            finalData = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var pagingResult = new PagingResult<T>(totalCount, pageNumber, pageSize);
            pagingResult.Items.AddRange(await finalData.ToListAsync());
            return pagingResult;
        }

        public static PagingResult<T> Create(IQueryable<T> data, int pageNumber, int pageSize)
        {
            IQueryable<T> finalData;
            var totalCount = data.Count();
            if (pageSize == -1)
            {
                finalData = data;
                var pagingResultFull = new PagingResult<T>(totalCount, pageNumber, pageSize);
                pagingResultFull.Items.AddRange(finalData.ToList());
                return pagingResultFull;
            }

            finalData = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var pagingResult = new PagingResult<T>(totalCount, pageNumber, pageSize);
            pagingResult.Items.AddRange(finalData.ToList());
            return pagingResult;
        }
    }
}
