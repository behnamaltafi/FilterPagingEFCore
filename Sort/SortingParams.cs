
using FilterPagingEfCore.Enums;

namespace FilterPagingEfCore.Sort
{
    public class SortingParams
    {

        public SortOrders SortOrder { get; set; } = SortOrders.Asc;
        public string ColumnName { get; set; } = string.Empty;
    }

}