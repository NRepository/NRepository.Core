namespace NRepository.Core.Tests
{
    using System.Collections.Generic;

    public abstract class Person
    {
        protected Person()
        {
        }

        protected Person(Names id, string title, string firstName, string lastName, string sortValue, bool isFemale = false)
        {
            Id = id;
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            SortValue = sortValue;
            IsFemale = isFemale;
        }

        public Names Id { get; set; }
        public bool IsFemale { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Parent Partner { get; set; }
        public Animal Pet { get; set; }
        public string SortValue { get; set; }
    }
}
