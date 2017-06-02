using System.Data.Entity;

namespace Snapshotter.Database
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext(DatabaseConnectionDetails connectionDetails) : base(connectionDetails.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            System.Data.Entity.Database.SetInitializer<DatabaseContext>(null);
        }
    }
}