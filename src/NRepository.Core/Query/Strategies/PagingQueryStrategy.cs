namespace NRepository.Core.Query
{
    using System;
    using System.Linq;

    public class PagingQueryStrategy : QueryStrategy
    {
        private bool _isReentrent;

        public PagingQueryStrategy(int page, int pageSize, bool getRowCount = false)
        {
            GetRowCount = getRowCount;

            if (pageSize < 1)
                throw new ArgumentException("pageSize cannot be less than 1", "pageSize");

            if (page < 0)
                throw new ArgumentException("page cannot be less than 0", "page");

            Page = page;
            PageSize = pageSize;
        }

        public PagingQueryStrategy(int page, int pageSize, out Func<int> rowCountCallback)
        {
            if (page < 0)
                throw new ArgumentException("page cannot be less than 0", "page");

            if (pageSize < 1)
                throw new ArgumentException("pageSize cannot be less than 1", "pageSize");

            Page = page;
            PageSize = pageSize;

            GetRowCount = true;
            rowCountCallback = () => RowCount.Value;
        }


        public int? RowCount
        {
            get;
            private set;
        }

        public int PageSize
        {
            get;

        }

        public int Page
        {
            get;

        }

        public bool GetRowCount
        {
            get;
            private set;
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            var query = QueryableRepository.GetQueryableEntities<T>(additionalQueryData);
            if (GetRowCount)
            {
                if (_isReentrent)
                    return query;

                _isReentrent = true;
                query = (IQueryable<T>)QueryableRepository.GetQueryableEntities<T>(additionalQueryData).ToArray().AsQueryable();
                RowCount = query.Count();
            }

            var skip = Page < 1 ? 0 : (Page - 1) * PageSize;
            var filteredQuery = query.Skip(skip).Take(PageSize);
            return filteredQuery;
        }
    }
}
