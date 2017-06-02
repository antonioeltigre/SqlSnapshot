using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Snapshotter.Database.Entities;
using Snapshotter.Database.SQL;

namespace Snapshotter.Database
{
    internal class DatabaseInfoRepo
    {
        private readonly DatabaseConnectionDetails _connectionDetails;
        private readonly SqlBuilder _sqlBuilder = new SqlBuilder();

        public DatabaseInfoRepo(DatabaseConnectionDetails connectionDetails)
        {
            _connectionDetails = connectionDetails;
        }

        public IEnumerable<Entities.Database> GetDatabases()
        {
            using (var context = new DatabaseContext(_connectionDetails))
            {
                return context.Database.SqlQuery<Entities.Database>("SELECT * FROM sys.databases WHERE sys.databases.database_id > 4 AND sys.databases.source_database_id IS NULL").ToList();
            }
        }

        public IEnumerable<File> GetDatabaseFiles(int databaseId)
        {
            using (var context = new DatabaseContext(_connectionDetails))
            {
                return context.Database.SqlQuery<File>("SELECT * FROM sys.master_files WHERE type_desc <> 'LOG' AND database_id = " + databaseId).ToList();
            }
        }

        public async Task CreateSnapshotAsync(int databaseId, string databaseName, string snapshotName)
        {
            var files = GetDatabaseFiles(databaseId);
            var sql = _sqlBuilder.CreateSnapshot(snapshotName, databaseName, files);
            
            using (var connection = new SqlConnection(_connectionDetails.ConnectionString))
            {
                connection.Open();
                var sqlCommand = connection.CreateCommand();

                sqlCommand.CommandText = sql;
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public IEnumerable<Entities.Database> GetSnapshots(int databaseId)
        {
            using (var context = new DatabaseContext(_connectionDetails))
            {
                return context.Database.SqlQuery<Entities.Database>("SELECT * FROM sys.databases WHERE sys.databases.source_database_id = " + databaseId).ToList();
            }
        }

        public async Task RestoreSnapshotAsync(string databaseName, string snapshotName)
        {
            using (var connection = new SqlConnection(_connectionDetails.ConnectionString))
            {
                connection.Open();
                var sqlCommand = connection.CreateCommand();

                sqlCommand.CommandText = _sqlBuilder.RestoreSnapshot(databaseName, snapshotName);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task DropSnapshotAsync(string snapshotName)
        {
            using (var connection = new SqlConnection(_connectionDetails.ConnectionString))
            {
                connection.Open();
                var sqlCommand = connection.CreateCommand();

                sqlCommand.CommandText = "DROP DATABASE " + snapshotName;
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
    }
}