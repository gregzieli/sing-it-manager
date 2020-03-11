using SongReader.Models;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SongReader.Services
{
    public class SongParser
    {
        private static readonly Regex Regex = new Regex(@"(.+?)(?:(?:,| &| y| f(?:ea)?t\.?) (.+))* - (.+)", RegexOptions.IgnoreCase);

        public Song ParseSong(string path)
        {
            var songDesignation = $"{path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1)}";

            var match = Regex.Match(songDesignation);
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid song designation: {songDesignation}", nameof(path));
            }

            var featured = match.Groups[2]?.Value;

            return new Song
            {
                Artist = match.Groups[1].Value,
                Name = match.Groups[3].Value,
                Featured = string.IsNullOrWhiteSpace(featured) ? default : featured
            };
        }
    }
}
