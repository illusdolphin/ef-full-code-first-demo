using EFCodeFirst.Enums;
using EFCodeFirst.Model;
using FirstQuad.Common.Helpers;
using System;
using System.Data.Entity;
using System.Linq;

namespace EFCodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new MyContextInitializer());

            using(var db = new MyContext())
            {
                ShowSummary(db);
                ShowOrdersReportJoin(db);
                ShowPersonsFilter(db);
                ShowPersonsGrouping(db);
                ShowOrdersGrouping(db);

                ShowNewOrderLazy(db);

                ShowLocalSetLookup(db);
                ShowChangeTracker(db);
            }
            
            Console.ReadKey();
        }

        private static void ShowSummary(MyContext db)
        {
            Console.WriteLine($"We have {db.Persons.Count()} persons and {db.Orders.Count()} orders");
        }

        private static void ShowOrdersReportJoin(MyContext db)
        {
            var reportOrders =
                from o in db.Orders
                join pCreated in db.Persons on o.CreatedByPersonId equals pCreated.Id

                join pProcessed in db.Persons on o.ProcessedByPersonId equals pProcessed.Id into pProcessedJoin
                from pProcessed in pProcessedJoin.DefaultIfEmpty()

                select new
                {
                    o.Id,
                    o.Comments,
                    CreatedByFirstName = pCreated.FirstName,
                    CreatedByLastName = pCreated.LastName,
                    ProcessedByFirstName = pProcessed.FirstName,
                    ProcessedByLastName = pProcessed.LastName
                };

            foreach (var rec in reportOrders)
            {
                Console.WriteLine($"{rec.Id}\t{rec.Comments}\t{rec.CreatedByFirstName} {rec.CreatedByLastName}\t\t\t{rec.ProcessedByFirstName} {rec.ProcessedByLastName}");
            }
            Console.WriteLine();
        }

        private static void ShowPersonsFilter(MyContext db)
        {
            var filter = PredicateBuilder.True<Person>();

            if (new Random().Next(1000) < 500)
            {
                var minBirthDay = new DateTime(2003, 01, 01);
                filter = filter.AndAlso(x => x.BirthDate > minBirthDay);
            }

            if (new Random().Next(1000) < 200)
            {
                var minBirthDay = new DateTime(2003, 01, 01);
                filter = filter.AndAlso(x => x.FirstName.Length > 10);
            }

            var persons =
                from p in db.Persons.Where(filter)
                orderby p.Id descending
                select p;

            foreach (var rec in persons)
            {
                Console.WriteLine($"{rec.Id}\t{rec.FirstName}\t{rec.LastName}");
            }
            Console.WriteLine();
        }

        private static void ShowPersonsGrouping(MyContext db)
        {
            var createdOrdersSummary =
                from o in db.Orders
                group o by o.CreatedByPersonId into oJoin
                select new
                {
                    PersonId = oJoin.Key,
                    Count = oJoin.Count()
                };

            var processedOrdersSummary =
                from o in db.Orders
                group o by o.ProcessedByPersonId into oJoin
                select new
                {
                    PersonId = oJoin.Key,
                    Count = oJoin.Count()
                };

            var persons =
                from p in db.Persons

                join co in createdOrdersSummary on p.Id equals co.PersonId into coJoin
                from co in coJoin.DefaultIfEmpty()

                join po in processedOrdersSummary on p.Id equals po.PersonId into poJoin
                from po in poJoin.DefaultIfEmpty()

                orderby po.Count descending, co.Count descending, p.LastName, p.FirstName
                select new
                {
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    CreatedOrdersCount = (int?)co.Count,
                    ProcessedOrders = (int?)po.Count
                };

            foreach (var rec in persons)
            {
                Console.WriteLine($"{rec.Id}\t{rec.FirstName}\t{rec.LastName}\t{rec.ProcessedOrders}\t{rec.CreatedOrdersCount}");
            }
            Console.WriteLine();
        }

        private static void ShowLocalSetLookup(MyContext db)
        {
            //risky
            var firstPerson = db.Persons.First();

            var anotherPerson = db.Persons.Where(x => x.Id == firstPerson.Id).ToList().First();

            if (!ReferenceEquals(firstPerson, anotherPerson))
                throw new ApplicationException($"Not expected {anotherPerson.Id} and {firstPerson.Id} to be the same reference");

            var firstPersonNoTracking = db.Persons.AsNoTracking().First();

            if (firstPersonNoTracking.Id != firstPerson.Id)
                throw new ApplicationException($"Not the same person, Expected: {firstPersonNoTracking.Id}, Actual: {firstPerson.Id}");

            if (ReferenceEquals(firstPersonNoTracking, firstPerson))
                throw new ApplicationException($"Expected {firstPersonNoTracking.Id} and {firstPerson.Id} to be the same reference");
        }

        private static void ShowChangeTracker(MyContext db)
        {
            db.ChangeTracker.DetectChanges();
            var hasChanges = db.ChangeTracker.HasChanges();
            ShowChanges(db);

            var anyOrder = db.Orders.First();
            anyOrder.Status = OrderStatus.Done;
            ShowChanges(db);
        }

        private static void ShowChanges(MyContext db)
        {
            var actualEntries = db.ChangeTracker.Entries().ToList();
            Console.WriteLine($"Actually {actualEntries.Count} entries");
            foreach (var entry in actualEntries)
            {
                if (entry.State == EntityState.Unchanged)
                    continue;

                Console.WriteLine($"Entry {entry.Entity.GetType().Name} / {entry.State}");

                var order = entry.Entity as Order;
                if (order != null)
                {
                    var statusName = nameof(Order.Status);
                    Console.WriteLine($"Order status: {entry.OriginalValues[statusName]} / {entry.CurrentValues[statusName]}");
                }
            }
        }


        private static void ShowNewOrderLazy(MyContext db)
        {
            var anyPerson = db.Persons.First();

            var newOrder = new Order
            {
                Id = 44,
                CreatedByPersonId = anyPerson.Id,
                ProcessedByPersonId = anyPerson.Id,
                Status = OrderStatus.Created,
                Comments = "New Order Test" //TODO: uncomment
            };
            db.Orders.Add(newOrder);
            db.SaveChanges();
        }

        private static void ShowOrdersGrouping(MyContext db)
        {
            var orderGrouped =
                from o in db.Orders
                join p in db.Persons on o.CreatedByPersonId equals p.Id

                group new { o, p } by new { p.BirthDate, o.Status } into oJoin

                select new
                {
                    oJoin.Key.Status,
                    oJoin.Key.BirthDate,
                    Orders = oJoin.Select(x => x.o)
                };

            foreach (var rec in orderGrouped)
            {
                Console.WriteLine($"{rec.Status}\t{rec.BirthDate}\t{rec.Orders.Count()}");
            }
        }
        
    }
}
