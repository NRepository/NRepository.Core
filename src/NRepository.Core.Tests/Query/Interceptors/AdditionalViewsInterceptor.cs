namespace NRepository.Core.Tests
{
    using System.Linq;
    using NRepository.Core.Query;

    public class AdditionalViewsInterceptor : IQueryInterceptor
    {
        public IQueryable<T> Query<T>(IQueryRepository repository, IQueryable<T> query, object additionalData) where T : class
        {
            if (typeof(T) == typeof(PersonProjection))
            {
                return CreatePersonProjection<T>(repository, additionalData);
            }

            return query;
        }

        private static IQueryable<T> CreatePersonProjection<T>(IQueryRepository repository, object additionalQueryData) where T : class
        {
            var families = from a in repository.GetEntities<Person>()
                           select new PersonProjection
                           {
                               FullName = a.FirstName + " " + a.LastName,
                               AdditionalQueryData = additionalQueryData != null ? 
                                    additionalQueryData.ToString() :
                                    null,
                           };

            // var families = from a in repository.GetEntities<Person>()
            //               where a.Partner != null && !a.IsFemale
            //               select new FamilyView
            //               {
            //                   Wife = a.Partner.Id,
            //                   WifeFullName = string.Format("{0} {1} {2}", a.Partner.Title, a.Partner.FirstName, a.Partner.LastName),
            //                   HusbandFullName = string.Format("{0} {1} {2}", a.Title, a.FirstName, a.LastName),
            //                   Husband = a.Id,
            //                   CombinedChildren = a.Children.Union(a.Partner.Children)
            //               };

            return (IQueryable<T>)families;
        }
    }
}
