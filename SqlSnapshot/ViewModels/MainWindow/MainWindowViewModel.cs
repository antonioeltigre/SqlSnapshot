using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SqlSnapshot.Annotations;

namespace SqlSnapshot.ViewModels.MainWindow
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _status;
        private List<DatabaseViewModel> _databases = new List<DatabaseViewModel>();
        private DatabaseViewModel _selectedDatabase;
        private SnapShotViewModel _selectedSnapshot;
        
        public List<DatabaseViewModel> Databases
        {
            get => _databases;
            set
            {
                if (Equals(value, _databases)) return;
                _databases = value;
                OnPropertyChanged();
            }
        }

        public DatabaseViewModel SelectedDatabase
        {
            get => _selectedDatabase;
            set
            {
                if (Equals(value, _selectedDatabase)) return;
                _selectedDatabase = value;
                OnPropertyChanged();
            }
        }

        public SnapShotViewModel SelectedSnapshot
        {
            get => _selectedSnapshot;
            set
            {
                if (Equals(value, _selectedSnapshot)) return;
                _selectedSnapshot = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (value == _status) return;
                _status = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}