namespace NRepository.Core.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PagingQueryStrategy : QueryStrategy
    {
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
            private set;
        }

        public int Page
        {
            get;
            private set;
        }

        public bool GetRowCount
        {
            get;
            private set;
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            if (GetRowCount)
            {
                GetRowCount = false;
                RowCount = QueryableRepository.GetQueryableEntities<T>(additionalQueryData).Count();
                return QueryableRepository.GetQueryableEntities<T>(additionalQueryData);
            }

            var skip = Page < 1 ? 0 : (Page - 1) * PageSize;
            var query = QueryableRepository.GetQueryableEntities<T>(additionalQueryData).Skip(skip).Take(PageSize);
            return query;
        }
    }

}
