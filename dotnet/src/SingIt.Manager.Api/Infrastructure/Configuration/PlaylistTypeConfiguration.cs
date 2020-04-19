using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using SingIt.Manager.Api.Models;

namespace SingIt.Manager.Api.Infrastructure.Configuration
{
    public static class PlaylistTypeConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Playlist>(initializer =>
            {
                initializer.AutoMap();

                initializer.MapIdMember(c => c.Id).SetIdGenerator(StringObjectIdGenerator.Instance);

                initializer.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
        }
    }
}
