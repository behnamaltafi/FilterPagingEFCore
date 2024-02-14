using FilterPagingEfCore.Enums;
using FilterPagingEfCore.Extenstion;

namespace FilterPagingEfCore.Filter
{
    public class Filter<T>
    {
        public static IQueryable<T> FilteredData(IEnumerable<FilterParam> filterParams, IQueryable<T> data, ComparisonMode comparisonMode)
        {
            return data.DynamicWhere(filterParams, comparisonMode);
        }
    }

}