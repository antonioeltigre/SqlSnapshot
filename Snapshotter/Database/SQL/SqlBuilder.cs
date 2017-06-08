using System.Collections.Generic;
using System.IO;
using System.Linq;
using File = Snapshotter.Database.Entities.File;

namespace Snapshotter.Database.SQL
{
    internal class SqlBuilder
    {
        public string CreateSnapshot(string snapshotName, string databaseName, IEnumerable<File> files)
        {
            var onStatements = files.Select(f => $@"(name = '{f.name}', FILENAME = '{GetSnapshotPath(f, snapshotName)}')");
            return $@"CREATE DATABASE {databaseName}_{snapshotName} ON {string.Join(",", onStatements)} AS SNAPSHOT OF {databaseName}";
        }

        private string GetSnapshotPath(File file, string snapshotName)
        {
            return Path.Combine(Path.GetDirectoryName(file.physical_name) ?? string.Empty, Path.GetFileNameWithoutExtension(file.physical_name) ?? string.Empty) + "_" + snapshotName + Path.GetExtension(file.physical_name);
        }

        public string RestoreSnapshot(string databaseName, string snapshotName)
        {
            return $@"USE master
                ALTER DATABASE {databaseName}
                SET SINGLE_USER WITH
                ROLLBACK IMMEDIATE
                RESTORE DATABASE {databaseName} FROM
                DATABASE_SNAPSHOT = '{snapshotName}'
                ALTER DATABASE {databaseName} SET MULTI_USER";
        }
    }
}