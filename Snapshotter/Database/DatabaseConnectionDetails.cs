namespace Snapshotter.Database
{
    public struct DatabaseConnectionDetails
    {
        public DatabaseConnectionDetails(string server, string userName, string password)
        {
            ConnectionString = $"data source={server};user id={userName};password={password};MultipleActiveResultSets=True";

        }

        public string ConnectionString { get; }
    }
}