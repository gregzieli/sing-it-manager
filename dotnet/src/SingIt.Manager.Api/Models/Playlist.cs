using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingIt.Manager.Api.Models
{
    public class Playlist
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}
