using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Snapshotter.Database.Entities;
using Snapshotter.Database.SQL;

namespace Snapshotter.Tests
{
    [TestFixture]
    public class SqlBuilderTests
    {
        [Test]
        public void TestCreateSnapshot()
        {
            var database = new Database.Entities.Database { name = "databaseName", database_id = 123};
            var files = new List<File>
            {
                new File {name = "fileA", physical_name = @"c:\database\a.mdf"},
                new File {name = "fileB", physical_name = @"c:\database\b.mdf"},
                new File {name = "fileC", physical_name = @"c:\database\c.mdf"}
            };

            var generatedSql = new SqlBuilder().CreateSnapshot("NewSnapshot", database.name, files);

            generatedSql.Should()
                .Be(
                    @"CREATE DATABASE databaseName_NewSnapshot ON (name = 'fileA', FILENAME = 'c:\database\a_NewSnapshot.mdf'),(name = 'fileB', FILENAME = 'c:\database\b_NewSnapshot.mdf'),(name = 'fileC', FILENAME = 'c:\database\c_NewSnapshot.mdf') AS SNAPSHOT OF databaseName");
        }

        [Test]
        public void TestRestoreSnapshot()
        {
            var generatedSql = new SqlBuilder().RestoreSnapshot("databaseToRestore", "snapshotToRestore");

            generatedSql.Should()
                .Be(
                    @"USE master
                ALTER DATABASE databaseToRestore
                SET SINGLE_USER WITH
                ROLLBACK AFTER 60
                RESTORE DATABASE databaseToRestore FROM
                DATABASE_SNAPSHOT = 'snapshotToRestore'
                ALTER DATABASE databaseToRestore SET MULTI_USER");
        }
    }
}