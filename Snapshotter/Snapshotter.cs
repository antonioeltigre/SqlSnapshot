﻿using System.Collections.Generic;
using System.Linq;
using Snapshotter.Database;

namespace Snapshotter
{
    public class Snapshotter
    {
        private readonly DatabaseInfoRepo _repo;

        public Snapshotter(DatabaseConnectionDetails connectionDetails)
        {
            _repo = new DatabaseInfoRepo(connectionDetails);
        }

        public IEnumerable<Domain.Database> GetDatabases()
        {
            return _repo.GetDatabases().Select(x => new Domain.Database(_repo) { Id = x.database_id, Name = x.name });
        }
    }
}