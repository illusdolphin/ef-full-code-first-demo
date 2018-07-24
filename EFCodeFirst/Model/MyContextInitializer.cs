using System;
using System.Collections.Generic;

namespace EFCodeFirst.Model
{
    /// <summary>
    /// From https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application
    /// </summary>
    public class MyContextInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MyContext>
    {
        protected override void Seed(MyContext context)
        {
            var persons = new List<Person>
            {
                new Person{FirstName = "Carson", LastName = "Alexander", BirthDate = DateTime.Parse("2005-09-01")},
                new Person{FirstName = "Meredith", LastName = "Alonso", BirthDate = DateTime.Parse("2002-09-01")},
                new Person{FirstName = "Arturo", LastName = "Anand", BirthDate = DateTime.Parse("2003-09-01")},
                new Person{FirstName = "Gytis", LastName = "Barzdukas", BirthDate = DateTime.Parse("2002-09-01")},
                new Person{FirstName = "Yan", LastName = "Li", BirthDate = DateTime.Parse("2002-09-01")},
                new Person{FirstName = "Peggy", LastName = "Justice", BirthDate = DateTime.Parse("2001-09-01")},
                new Person{FirstName = "Laura", LastName = "Norman", BirthDate = DateTime.Parse("2003-09-01")},
                new Person{FirstName = "Nino", LastName = "Olivetto", BirthDate = DateTime.Parse("2005-09-01")}
            };

            persons.ForEach(s => context.Persons.Add(s));

            var orders = new List<Order>
            {
                new Order{ CreatedByPerson = persons[0], ProcessedByPerson = persons[7], Comments = "Chemistry"},
                new Order{ CreatedByPerson = persons[1], ProcessedByPerson = persons[6], Comments = "Microeconomics"},
                new Order{ CreatedByPerson = persons[2], ProcessedByPerson = persons[5], Comments = "Macroeconomics"},
                new Order{ CreatedByPerson = persons[3], ProcessedByPerson = persons[6], Comments = "Calculus"},
                new Order{ CreatedByPerson = persons[4], ProcessedByPerson = persons[7], Comments = "Trigonometry"},
                new Order{ CreatedByPerson = persons[5], ProcessedByPerson = null, Comments = "Composition"},
                new Order{ CreatedByPerson = persons[6], ProcessedByPerson = null, Comments = "Literature"}
            };
            orders.ForEach(s => context.Orders.Add(s));
            context.SaveChanges();
        }
    }
}
