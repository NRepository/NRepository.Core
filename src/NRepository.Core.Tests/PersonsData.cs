namespace NRepository.Core.Tests
{
    using NRepository.Core.Tests;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class PersonsData
    {
        public static List<Person> Data
        {
            get
            {
                var persons = new Person[]
                {
                    // Order by 
                    new Parent(Names.IsabelleOsborne, "Ms", "Isabelle", "Osborne", "G",true),
                    new Parent(Names.JohnKelly, "Mr", "John", "Kelly", "Z"),
                    new Child(Names.MarcBurgess, "Mr", "Marc", "Burgess", "A"),
                    new Parent(Names.PaulCox, "Mr", "Paul", "Cox", "B"),
                    new Parent(Names.NigelBurgess, "Mr", "Nigel", "Burgess", "C"),
                    new Child(Names.TomCox, "Mr", "Tom", "Cox", "D"),
                    new Parent(Names.SueCox, "Mrs", "Sue", "Cox", "E", true),
                    new Parent(Names.JeanetteBurgess, "Mrs", "Jeanette", "Burgess", "F",true),
                    new Child(Names.EllieOsborne, "Miss", "Ellie", "Osborne", "H",true),
                    new Child(Names.AimmeOsborne, "Miss", "Aimee", "Osborne", "I",true),
                    new Child(Names.ToBeDecided,"To", "Be", "Decided", "J",true),
                };

                return persons.ToList();
            }
        }
        
    }
}
