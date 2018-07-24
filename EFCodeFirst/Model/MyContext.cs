using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EFCodeFirst.Model
{
    public class MyContext : DbContext
    {
        public MyContext() : base("MyContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
