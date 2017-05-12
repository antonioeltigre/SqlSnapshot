using System.Collections.Generic;
using System.Linq;
using Snapshotter.Database;

namespace Snapshotter
{
    public class Snapshotter
    {
        private readonly DatabaseInfoRepo _repo;

        public Snapshotter()
        {
            _repo = new DatabaseInfoRepo();
        }

        public IEnumerable<Domain.Database> GetDatabases()
        {
            return _repo.GetDatabases().Select(x => new Domain.Database(_repo) { Id = x.database_id, Name = x.name });
        }
    }
    
    namespace Domain
    {
        public class Database
        {
            private readonly DatabaseInfoRepo _repo;

            internal Database(DatabaseInfoRepo repo)
            {
                _repo = repo;
            }

            public int Id { get; internal set; }

            public string Name { get; internal set; }

            public List<Snapshot> GetSnapshots()
            {
                return _repo.GetSnapshots(Id).Select(x => new Snapshot { Name = x.name, Id = x.database_id}).ToList();
            }
        }
    }
}

namespace Snapshotter.Domain
{
    public class Snapshot
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}