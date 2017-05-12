using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Snapshotter.Domain;

namespace SqlSnapshot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _viewModel;
            var snapshotter = new Snapshotter.Snapshotter();

            _viewModel.Databases = snapshotter.GetDatabases().Select(x => new DatabaseViewModel {Database = x, Name = x.Name, Id = x.Id}).ToList();
            _viewModel.Names = _viewModel.Databases.Select(x => x.Name).ToList();
        }
    }
}

public class MainWindowViewModel
{
    public List<DatabaseViewModel> Databases { get; set; }
    public List<string> Names { get; set; }

    public DatabaseViewModel SelectedDatabase { get; set; }

    public SnapShotViewModel SelectedSnapshot { get; set; }
}

public class DatabaseViewModel
{
    public Database Database { get; set; }

    public int Id { get; set; }

    public string Name { get; set; }

    public List<SnapShotViewModel> SnapShots => Database.GetSnapshots().Select(x => new SnapShotViewModel {Name = x.Name}).ToList();
}

public class SnapShotViewModel
{
    public string Name { get; set; }
}