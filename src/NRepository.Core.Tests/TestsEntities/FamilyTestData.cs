namespace NRepository.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Query;

    public static class FamilyTestData
    {
        public const string RabittName = "Thumper";
        public const string CatName = "Cat1";

        public static IEnumerable<object> GetData()
        {
            var retVal = new List<object>();

            var persons = new Person[]
                {
                    // Order by 
                    new Parent(Names.JohnKelly, "Mr", "John", "Kelly", "Z"),
                    new Child(Names.MarcBurgess, "Mr", "Marc", "Burgess", "A"),
                    new Parent(Names.PaulCox, "Mr", "Paul", "Cox", "B"),
                    new Parent(Names.NigelBurgess, "Mr", "Nigel", "Burgess", "C"),
                    new Child(Names.TomCox, "Mr", "Tom", "Cox", "D"),
                    new Parent(Names.SueCox, "Mrs", "Sue", "Cox", "E", true),
                    new Parent(Names.JeanetteBurgess, "Mrs", "Jeanette", "Burgess", "F",true),
                    new Parent(Names.IsabelleOsborne, "Ms", "Isabelle", "Osborne", "G",true),
                    new Child(Names.EllieOsborne, "Miss", "Ellie", "Osborne", "H",true),
                    new Child(Names.AimmeOsborne, "Miss", "Aimee", "Osborne", "I",true),
                    new Child(Names.ToBeDecided,"To", "Be", "Decided", "J",true),
                };

            Func<Names, Person> GetPerson = name => persons.Single(p => p.Id == (Names)name);

            // Kelbornes
            //GetPerson(Names.JohnKelly).Partner = GetPerson(Names.IsabelleOsborne);
            //GetPerson(Names.IsabelleOsborne).Partner = GetPerson(Names.JohnKelly);
            //GetPerson(Names.IsabelleOsborne).Children.Add(GetPerson(Names.EllieOsborne));
            //GetPerson(Names.IsabelleOsborne).Children.Add(GetPerson(Names.AimmeOsborne));
            //GetPerson(Names.JohnKelly).Children.Add(GetPerson(Names.ToBeDecided));

            //// Cox's
            //GetPerson(Names.SueCox).Partner = GetPerson(Names.PaulCox);
            //GetPerson(Names.PaulCox).Partner = GetPerson(Names.SueCox);
            //GetPerson(Names.PaulCox).Children.Add(GetPerson(Names.TomCox));

            //// Burgess's
            //GetPerson(Names.JeanetteBurgess).Partner = GetPerson(Names.NigelBurgess);
            //GetPerson(Names.NigelBurgess).Partner = GetPerson(Names.JeanetteBurgess);
            //GetPerson(Names.JeanetteBurgess).Children.Add(GetPerson(Names.MarcBurgess));
            //GetPerson(Names.NigelBurgess).Children.Add(GetPerson(Names.MarcBurgess));

            // Pet rescue
            var thumper = new Rabitt { Name = RabittName };
            var cat = new Cat { Name = CatName };

            GetPerson(Names.JeanetteBurgess).Pet = thumper;
            GetPerson(Names.SueCox).Pet = cat;

            retVal.AddRange(persons);
            retVal.Add(thumper);
            retVal.Add(cat);

            return retVal;
        }
    }
}
