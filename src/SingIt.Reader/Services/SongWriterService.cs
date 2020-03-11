using Newtonsoft.Json;
using Refit;
using SongReader.Contracts;
using SongReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SongReader.Services
{
    public class SongWriterService
    {
        public async Task WriteToDisk(IEnumerable<Song> songs, string outputDir)
        {
            var path = Path.Combine(
                string.IsNullOrEmpty(outputDir)
                    ? Environment.GetEnvironmentVariable("TEMP")
                    : outputDir,
                $"sing-it-songs-{DateTime.UtcNow:yyyyMMddTHHmmss}.json");

            var json = JsonConvert.SerializeObject(songs, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            await File.WriteAllTextAsync(path, json);

            Console.WriteLine($"Created a JSON file {path} with {songs.Count()} songs.");
        }

        public async Task WriteToDatabase(IEnumerable<Song> songs, string url)
        {
            var songApi = RestService.For<ISongApi>(url);
            await songApi.RefreshAsync(songs);

            Console.WriteLine($"Refreshed {songs.Count()} songs in the database.");
        }
    }
}
