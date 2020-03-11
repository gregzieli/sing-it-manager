using Refit;
using SongReader.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongReader.Contracts
{
    internal interface ISongApi
    {
        [Post("/api/songs/refresh")]
        Task RefreshAsync([Body] IEnumerable<Song> songs);
    }
}
