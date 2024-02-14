using FilterPagingEfCore.Enums;
using FilterPagingEfCore.Filter;
using FilterPagingEfCore.Sort;
using System.Linq.Expressions;
using System.Reflection;

namespace FilterPagingEfCore.Paging
{
    public class PagingParam
    {
        #region Input Variables
        private int pageNumber = 1;

        private int pageSize = 10;

        public List<SortingParams>? SortingParams { set; get; }

        public List<FilterParam>? FilterParam { get; set; }

        public ComparisonMode ComparisonMode { get; set; } = ComparisonMode.And;


        public int PageNumber
        {
            get => pageNumber;
            set
            {
                pageNumber = value < 1 ? 1 : value;
            }
        }

        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = value > 1 || value == -1 ? value : 10;
            }
        }
        #endregion


    }


}
