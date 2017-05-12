using System.Threading.Tasks;
using Snapshotter.Domain;

namespace SqlSnapshot.ViewModels.MainWindow
{
    public class SnapShotViewModel
    {
        public string Name { get; set; }

        public Snapshot SnapshotDomainObject { get; set; }
    }
}