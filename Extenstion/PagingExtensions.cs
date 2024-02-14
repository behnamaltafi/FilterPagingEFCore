using FilterPagingEfCore.Filter;
using FilterPagingEfCore.Paging;
using FilterPagingEfCore.Sort;

namespace FilterPagingEfCore.Extenstion
{
    public static class PagingExtensions
    {

        public static async Task<PagingResult<T>> FilterPaging<T>(this IQueryable<T> data, PagingParam pagingFilter) where T : class
        {
            #region [Filter]
            if (pagingFilter.FilterParam != null && pagingFilter.FilterParam.Any())
                data = Filter<T>.FilteredData(pagingFilter.FilterParam, data, pagingFilter.ComparisonMode) ?? data;
            #endregion

            #region [Sorting]     
            if (pagingFilter.SortingParams != null && pagingFilter.SortingParams.Any())
                data = Sorting<T>.SortData(data, pagingFilter.SortingParams);
            #endregion

            #region [Paging]
            return await PagingResult<T>.CreateAsync(data, pagingFilter.PageNumber, pagingFilter.PageSize);
            #endregion
        }
    }
}
