namespace NRepository.Core.Tests
{
    using System.Collections.Generic;

    public class Parent : Person
    {
        public Parent()
        {
            Children = new List<Child>();
        }

        public Parent(Names id, string title, string firstName, string lastName, string sortValue, bool isFemale = false)
            : base(id, title, firstName, lastName, sortValue, isFemale)
        {
            Children = new List<Child>();
        }

        public virtual ICollection<Child> Children { get; set; }

    }
}
