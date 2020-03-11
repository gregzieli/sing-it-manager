using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using SingIt.Manager.Api.Models;

namespace SingIt.Manager.Api.Infrastructure.Configuration
{
    public static class SongTypeConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Song>(initializer =>
            {
                initializer.AutoMap();

                initializer.MapIdMember(c => c.Id).SetIdGenerator(StringObjectIdGenerator.Instance);

                initializer.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));

                initializer.MapMember(c => c.Featured).SetIgnoreIfDefault(true);
            });
        }
    }
}
