using SongReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SongReader.Services
{
    public class SongReaderService
    {
        private readonly SongParser _songParser;

        public SongReaderService()
        {
            _songParser = new SongParser();
        }

        public IEnumerable<Song> Read(string path)
        {
            var directories = new List<string>(Directory.EnumerateDirectories(path));

            Console.WriteLine($"{directories.Count} directories found.");

            var songs = new List<Song>();
            foreach (var directoryPath in directories)
            {
                try
                {
                    songs.Add(_songParser.ParseSong(directoryPath));
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }

            Console.WriteLine($"{songs.Count()} songs successfully parsed.");

            return songs;
        }
    }
}
