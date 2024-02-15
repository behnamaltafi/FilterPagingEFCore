using FilterPagingEfCore.Enums;
using FilterPagingEfCore.Extenstion;

namespace FilterPagingEfCore.Filter
{
    public class FilterParam
    {

        public string ColumnName { get; set; } = string.Empty;
        public string FilterValue { get; set; } = string.Empty;
        public ComparisonMethod FilterOption { get; set; } = ComparisonMethod.Contain;
    }

}