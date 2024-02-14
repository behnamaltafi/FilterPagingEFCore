using FilterPaging.Paging.Enums;
using FilterPagingEfCore.Enums;
using System.Linq.Expressions;
using System.Reflection;

namespace FilterPagingEfCore.Sort
{
    public class Sorting<T>
    {
        public static IQueryable<T> GroupingData(IQueryable<T> data, IEnumerable<string> groupingColumns)
        {
            IOrderedQueryable<T> groupedData = null;


            foreach (var grpCol in groupingColumns.Where(x => !string.IsNullOrEmpty(x)))
            {
                var col = typeof(T).GetProperty(grpCol,
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (col != null)
                {
                    var param = Expression.Parameter(typeof(T));
                    var expr = Expression.Lambda<Func<T, object>>(
                        Expression.Convert(Expression.Property(param, col), typeof(object)), param);
                    groupedData = groupedData == null
                        ? data.OrderBy(expr)
                        : groupedData.ThenBy(expr);
                }
            }

            return groupedData ?? data;
        }

        public static IQueryable<T> SortData(IQueryable<T> data, IEnumerable<SortingParams> sortingParams)
        {

            IOrderedQueryable<T> sortedData = null;

            foreach (var sortingParam in sortingParams.Where(x => !string.IsNullOrEmpty(x.ColumnName)))
            {
                var col = typeof(T).GetProperty(sortingParam.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (col != null)
                {
                    var param = Expression.Parameter(typeof(T));
                    var expr = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(param, col), typeof(object)), param);
                    sortedData = sortedData == null ? sortingParam.SortOrder == SortOrders.Asc ? data.OrderBy(expr)
                        : data.OrderByDescending(expr)
                        : sortingParam.SortOrder == SortOrders.Asc ? sortedData.ThenBy(expr)
                        : sortedData.ThenByDescending(expr);
                }
            }
            return sortedData ?? data;
        }
    }

}