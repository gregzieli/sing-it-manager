using SingIt.Reader.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SingIt.Reader.Abstractions
{
    public interface ISongWriter
    {
        Task WriteAsync(IEnumerable<Song> songs, CancellationToken cancellationToken);
    }
}
