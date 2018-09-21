using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DemoApp.Model;

namespace DemoApp.DataAccess
{
    public class ApplicContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Child> Childs { get; set; }

        public ApplicContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Conventions
                .Remove<PluralizingTableNameConvention>();
        }
    }
}
