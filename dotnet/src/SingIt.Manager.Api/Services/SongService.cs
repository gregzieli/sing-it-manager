using MongoDB.Driver;
using SingIt.Manager.Api.Infrastructure;
using SingIt.Manager.Api.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SingIt.Manager.Api.Services
{
    public class SongService
    {
        private readonly ManagerContext _managerContext;

        public SongService(ManagerContext managerContext)
        {
            _managerContext = managerContext;
        }

        public async Task<IEnumerable<Song>> GetAsync()
        {
            return await _managerContext.Songs.Find(_ => true).ToListAsync();
        }

        public async Task<Song> GetAsync(string id)
        {
            return await _managerContext.Songs.Find(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async ValueTask<bool> CreateAsync(Song song)
        {
            var dbSong = await _managerContext.Songs.Find(x =>
                x.Title == song.Title &&
                x.Artist == song.Artist &&
                x.Featured == song.Featured)
                .FirstOrDefaultAsync();

            if (dbSong == default)
            {
                await _managerContext.Songs.InsertOneAsync(song);
                return true;
            }

            song.Id = dbSong.Id;
            return false;
        }

        public async Task RefreshAsync(IEnumerable<Song> songs, CancellationToken cancellationToken = default)
        {
            await _managerContext.Songs.DeleteManyAsync(_ => true, cancellationToken);

            await _managerContext.Songs.InsertManyAsync(songs, cancellationToken: cancellationToken);
        }
    }
}
