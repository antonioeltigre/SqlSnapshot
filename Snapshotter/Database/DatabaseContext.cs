using System.Data.Entity;

namespace Snapshotter.Database
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("data source=.;user id=sa;password=Bgt67yhn;MultipleActiveResultSets=True")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            System.Data.Entity.Database.SetInitializer<DatabaseContext>(null);
        }
    }
}