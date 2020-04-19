using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SingIt.Reader.Abstractions;
using SingIt.Reader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SingIt.Reader.Services
{
    public class MongoWriter : ISongWriter
    {
        private readonly string _connectionString;

        public MongoWriter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task WriteAsync(IEnumerable<Song> songs, CancellationToken cancellationToken)
        {
            ConfigureDatabase();

            var mongoUrl = MongoUrl.Create(_connectionString);

            var db = new MongoClient(mongoUrl).GetDatabase(mongoUrl.DatabaseName);
            var songCollection = db.GetCollection<Song>("songs");

            await songCollection.DeleteManyAsync(_ => true, cancellationToken);
            await songCollection.InsertManyAsync(songs, cancellationToken: cancellationToken);

            Console.WriteLine($"Refreshed {songs.Count()} songs in the database.");
        }

        private void ConfigureDatabase()
        {
            ConventionRegistry.Register("Camel Case", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);
            BsonClassMap.RegisterClassMap<Song>(initializer =>
            {
                initializer.AutoMap();
                initializer.MapMember(c => c.Featured).SetIgnoreIfDefault(true);
            });
        }
    }
}
