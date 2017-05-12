using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snapshotter.Database;

namespace Snapshotter.Domain
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

        public IEnumerable<Snapshot> GetSnapshots()
        {
            return _repo.GetSnapshots(Id).Select(x => new Snapshot(this, _repo) { Name = x.name, Id = x.database_id}).ToList();
        }

        public async Task CreateSnapshotAsync()
        {
            var snapshotCount = GetSnapshots().Count();
            await _repo.CreateSnapshotAsync(this.Id, this.Name, "snapshot" + snapshotCount);
        }
    }
}