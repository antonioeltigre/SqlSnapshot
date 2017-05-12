using System.Threading.Tasks;
using Snapshotter.Database;

namespace Snapshotter.Domain
{
    public class Snapshot
    {
        private readonly Database _database;
        private readonly DatabaseInfoRepo _repo;

        internal Snapshot(Database database, DatabaseInfoRepo repo)
        {
            _database = database;
            _repo = repo;
        }

        public string Name { get; set; }

        public int Id { get; set; }

        public async Task RestoreAsync()
        {
            await _repo.RestoreSnapshotAsync(_database.Name, Name);
        }

        public async Task DropAsync()
        {
            await _repo.DropSnapshotAsync(Name);
        }
    }
}