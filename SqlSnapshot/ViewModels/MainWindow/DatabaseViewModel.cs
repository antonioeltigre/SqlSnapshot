using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Snapshotter.Domain;
using SqlSnapshot.Annotations;

namespace SqlSnapshot.ViewModels.MainWindow
{
    public class DatabaseViewModel : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        public Database DatabaseDomainObject { get; set; }

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public List<SnapShotViewModel> SnapShots => DatabaseDomainObject.GetSnapshots()
            .Select(x => new SnapShotViewModel { Name = x.Name, SnapshotDomainObject = x })
            .ToList();

        public void RefreshSnapshots()
        {
            OnPropertyChanged(nameof(SnapShots));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}