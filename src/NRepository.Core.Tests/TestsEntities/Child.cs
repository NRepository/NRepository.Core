namespace NRepository.Core.Tests
{
    using System.Collections.Generic;

    public class Child : Person
    {
        private Child()
        {
        }

        public Child(Names id, string title, string firstName, string lastName, string sortValue, bool isFemale = false)
            : base(id, title, firstName, lastName, sortValue, isFemale)
        {
        }
    }
}
