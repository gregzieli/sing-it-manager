using MongoDB.Driver;
using SingIt.Manager.Api.Models;

namespace SingIt.Manager.Api.Infrastructure
{
    public class ManagerContext
    {
        private readonly IMongoDatabase _db;

        public ManagerContext(IMongoDatabase database)
        {
            _db = database;
        }

        public IMongoCollection<Song> Songs => _db.GetCollection<Song>("songs");
    }
}
