using System;
using System.Linq;
using NUnit.Framework;
using Snapshotter.Database;

namespace Snapshotter.Tests
{
    [TestFixture]
    public class RepoTests
    {
        [Test]
        public void databases()
        {
            new DatabaseInfoRepo().GetDatabases().ToList().ForEach(d => Console.WriteLine(d.name));
        }

        [Test]
        public void files()
        {
            new DatabaseInfoRepo().GetDatabaseFiles(5).ToList().ForEach(d => Console.WriteLine(d.name + d.physical_name));
        }

        [Test]
        public void createsnapshot()
        {
            var database = new DatabaseInfoRepo().GetDatabases().Single(x => x.database_id == 5);
            new DatabaseInfoRepo().CreateSnapshot(database, "simonTest2");
        }

        [Test]
        public void getSnapshots()
        {
            var enumerable = new DatabaseInfoRepo().GetSnapshots(5).ToList();
            enumerable.ForEach(s => Console.WriteLine(s.name));
        }

        [Test]
        public void dropSnapshot()
        {
            new DatabaseInfoRepo().DropSnapshot("TESTDB_MSH_TRUNK_simonTest2");
        }

        [Test]
        public void restoreSnapshot()
        {
            var database = new DatabaseInfoRepo().GetDatabases().Single(x => x.database_id == 5);
            var snapshot = new DatabaseInfoRepo().GetSnapshots(5).ToList().First();
            new DatabaseInfoRepo().RestoreSnapshot(database, snapshot);
        }
    }
}