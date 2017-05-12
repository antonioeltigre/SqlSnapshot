using System.Collections.Generic;
using System.IO;
using System.Linq;
using File = Snapshotter.Database.Entities.File;

namespace Snapshotter.Database.SQL
{
    internal class SqlBuilder
    {
        public string CreateSnapshot(string snapshotName, Entities.Database database, IEnumerable<File> files)
        {
            var onStatements = files.Select(f => $@"(name = '{f.name}', FILENAME = '{GetSnapshotPath(f, snapshotName)}')");
            return $@"CREATE DATABASE {database.name}_{snapshotName} ON {string.Join(",", onStatements)} AS SNAPSHOT OF {database.name}";
        }

        private object GetSnapshotPath(File file, string snapshotName)
        {
            return Path.GetPathRoot(file.physical_name) + Path.GetFileNameWithoutExtension(file.physical_name) + "_" + snapshotName + Path.GetExtension(file.physical_name);
        }



        public string RestoreSnapshot(string databaseName, string snapshotName)
        {
            return $@"USE master
                ALTER DATABASE {databaseName}
                SET SINGLE_USER WITH
                ROLLBACK AFTER 60
                RESTORE DATABASE {databaseName} FROM
                DATABASE_SNAPSHOT = '{snapshotName}'
                ALTER DATABASE {databaseName} SET MULTI_USER";

        }


        /*
                USE master;
                ALTER DATABASE TESTDB_MSH_TRUNK
                    SET SINGLE_USER WITH
                ROLLBACK AFTER 60 --this will give your current connections 60 seconds to complete
                    RESTORE DATABASE TESTDB_MSH_TRUNK FROM
                    DATABASE_SNAPSHOT = 'TESTDB_MSH_TRUNK_simonTest';
                GO
                    ALTER DATABASE TESTDB_MSH_TRUNK SET MULTI_USER
                GO*/
    }
}