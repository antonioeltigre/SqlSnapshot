using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Snapshotter.Database;
using SqlSnapshot.ViewModels.MainWindow;

namespace SqlSnapshot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;
            Refresh();
        }

        protected override void OnClosed(EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private async void SnapshotClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount != 2) return;

                switch (e.ChangedButton)
                {
                    case MouseButton.Left:
                        await RestoreSnapshot();
                        break;
                    case MouseButton.Right:
                        await DropSnapshot();
                        break;
                }
            }
            catch (Exception exception)
            {
                _viewModel.Status = exception.ToString();
            }
        }

        private async System.Threading.Tasks.Task DropSnapshot()
        {
            _viewModel.Status = "Deleting snapshot...";
            await _viewModel.SelectedSnapshot.SnapshotDomainObject.DropAsync();
            _viewModel.SelectedDatabase.RefreshSnapshots();
            _viewModel.Status = "Snapshot deleted";
        }

        private async System.Threading.Tasks.Task RestoreSnapshot()
        {
            _viewModel.Status = "Restoring snapshot...";
            await _viewModel.SelectedSnapshot.SnapshotDomainObject.RestoreAsync();
            _viewModel.SelectedDatabase.RefreshSnapshots();
            _viewModel.Status = "Snapshot restored";
        }

        private async void DatabaseClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount != 2) return;

                _viewModel.Status = "Creating snapshot...";
                await _viewModel.SelectedDatabase.DatabaseDomainObject.CreateSnapshotAsync();
                _viewModel.SelectedDatabase.RefreshSnapshots();
                _viewModel.Status = "Snapshot created";

            }
            catch (Exception exception)
            {
                _viewModel.Status = exception.ToString();
            }
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            try
            {
                _viewModel.Databases = new List<DatabaseViewModel>();
                var snapshotter =
                    new Snapshotter.Snapshotter(
                        new DatabaseConnectionDetails(Properties.Settings.Default.Server, Properties.Settings.Default.Username, Properties.Settings.Default.Password));
                _viewModel.Databases = snapshotter.GetDatabases()
                    .OrderBy(database => database.Name)
                    .Select(x => new DatabaseViewModel {DatabaseDomainObject = x, Name = x.Name, Id = x.Id}).ToList();
                _viewModel.Status = "Refreshed server";
            }
            catch (Exception exception)
            {
                _viewModel.Status = exception.ToString();
            }
        }
    }
}