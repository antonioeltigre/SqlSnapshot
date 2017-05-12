using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Snapshotter.Database.Entities;
using Snapshotter.Database.SQL;

namespace Snapshotter.Database
{
    internal class DatabaseInfoRepo
    {
        private const string ConnectionString = "data source=.;user id=sa;password=Bgt67yhn;MultipleActiveResultSets=True";
        private readonly SqlBuilder _sqlBuilder = new SqlBuilder();

        public IEnumerable<Entities.Database> GetDatabases()
        {
            using (var context = new DatabaseContext())
            {
                return context.Database.SqlQuery<Entities.Database>("SELECT * FROM sys.databases WHERE sys.databases.database_id > 4 AND sys.databases.source_database_id IS NULL").ToList();
            }
        }

        public IEnumerable<File> GetDatabaseFiles(int databaseId)
        {
            using (var context = new DatabaseContext())
            {
                return context.Database.SqlQuery<File>("SELECT * FROM sys.master_files WHERE type_desc <> 'LOG' AND database_id = " + databaseId).ToList();
            }
        }

        public void CreateSnapshot(Entities.Database database, string snapshotName)
        {
            var files = GetDatabaseFiles(database.database_id);
            var sql = _sqlBuilder.CreateSnapshot(snapshotName, database, files);
            
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var sqlCommand = connection.CreateCommand();

                sqlCommand.CommandText = sql;
                sqlCommand.ExecuteNonQuery();
            }
        }

        public IEnumerable<Entities.Database> GetSnapshots(int databaseId)
        {
            using (var context = new DatabaseContext())
            {
                return context.Database.SqlQuery<Entities.Database>("SELECT * FROM sys.databases WHERE sys.databases.source_database_id = " + databaseId).ToList();
            }
        }

        public void RestoreSnapshot(Entities.Database database, Entities.Database snapshot)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var sqlCommand = connection.CreateCommand();

                sqlCommand.CommandText = _sqlBuilder.RestoreSnapshot(database.name, snapshot.name);
                sqlCommand.ExecuteNonQuery();
            }
        }

        public void DropSnapshot(string snapshotName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var sqlCommand = connection.CreateCommand();

                sqlCommand.CommandText = "DROP DATABASE " + snapshotName;
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}