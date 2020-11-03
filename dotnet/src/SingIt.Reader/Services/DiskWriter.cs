using Newtonsoft.Json;
using SingIt.Reader.Abstractions;
using SingIt.Reader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SingIt.Reader.Services
{
    public class DiskWriter : ISongWriter
    {
        private readonly string _outputDirectory;

        public DiskWriter(string outputDirectory)
        {
            _outputDirectory = outputDirectory;
        }

        public async Task WriteAsync(IEnumerable<Song> songs, CancellationToken cancellationToken)
        {
            var path = Path.Combine(
                string.IsNullOrEmpty(_outputDirectory)
                    ? Environment.GetEnvironmentVariable("TEMP")
                    : _outputDirectory,
                $"sing-it-songs-{DateTime.UtcNow:yyyyMMddTHHmmss}.json");

            var json = JsonConvert.SerializeObject(songs, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            await File.WriteAllTextAsync(path, json, cancellationToken);

            Console.WriteLine($"Created a JSON file {path} with {songs.Count()} songs.");
        }
    }
}
