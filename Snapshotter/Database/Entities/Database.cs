using System.ComponentModel.DataAnnotations;

namespace Snapshotter.Database.Entities
{
    internal class Database
    {
        [Key]
        public int database_id { get; set; }

        public string name { get; set; }
    }
}
