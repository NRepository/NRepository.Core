namespace NRepository.Core.Tests
{
    using NRepository.Core;

    public static class PersonIncludes
    {
        public static readonly string Id = PropertyInfo<Person>.GetMemberName(p => p.Id);
        public static readonly string IsFemale = PropertyInfo<Person>.GetMemberName(p => p.IsFemale);
        public static readonly string Title = PropertyInfo<Person>.GetMemberName(p => p.Title);
        public static readonly string FirstName = PropertyInfo<Person>.GetMemberName(p => p.FirstName);
        public static readonly string LastName = PropertyInfo<Person>.GetMemberName(p => p.LastName);
        public static readonly string Partner = PropertyInfo<Person>.GetMemberName(p => p.Partner);
        public static readonly string Pet = PropertyInfo<Person>.GetMemberName(p => p.Pet);
        public static readonly string SortValue = PropertyInfo<Person>.GetMemberName(p => p.SortValue);
    }
}
