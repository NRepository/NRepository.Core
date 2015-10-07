namespace NRepository.Core.Tests
{
    using NRepository.Core;

    public static class PersonIncludes
    {
        private static Person _Person = default(Person);
        public static readonly string Id = nameof(_Person.Id);
        public static readonly string IsFemale = nameof(_Person.IsFemale);
        public static readonly string Title = nameof(_Person.Title);
        public static readonly string FirstName = nameof(_Person.FirstName);
        public static readonly string LastName = nameof(_Person.LastName);
        public static readonly string Partner = nameof(_Person.Partner);
        public static readonly string Pet = nameof(_Person.Pet);
        public static readonly string SortValue = nameof(_Person.SortValue);
    }
}
