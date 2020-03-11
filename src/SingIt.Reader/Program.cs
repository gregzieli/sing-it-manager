using Microsoft.Extensions.Configuration;
using SongReader.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SongReader
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var path = configuration["InputDirectory"];

            if (path == default)
            {
                Console.WriteLine("Path to the directory to read:");
                path = Console.ReadLine();
            }

            var useDb = configuration.GetValue<bool>("UseDB");

            Console.WriteLine($"The persistence method is the {(useDb ? "database" : "disk")}.");
            Console.WriteLine("Press enter to proceed, any other key to switch.");

            if (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter)
            {
                useDb = !useDb;
            }

            try
            {
                var songs = new SongReaderService().Read(path);

                var writer = new SongWriterService();

                if (useDb)
                {
                    await writer.WriteToDatabase(songs, configuration["ServiceEndpoints:SongApi"]);
                }
                else
                {
                    await writer.WriteToDisk(songs, configuration["OutputDirectory"]);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.ReadLine();
                return;
            }
        }
    }
}
