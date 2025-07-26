using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using DistributedFileStorage.Core;
using DistributedFileStorage.Infrastructure;
using DistributedFileStorage.Persistence;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog((_, cfg) => cfg.WriteTo.Console())
    .ConfigureServices(services =>
    {
        var dbPath = Path.Combine(Environment.CurrentDirectory, "chunks.db");
        services.AddDbContext<ChunkDbContext>(opt => opt.UseSqlite($"Data Source={dbPath}"));
        services.AddScoped<IFileChunker, DynamicFileChunker>();
        services.AddScoped<ChunkRepository>();
        services.AddScoped<IChunkRepository>(sp => sp.GetRequiredService<ChunkRepository>());
        services.AddScoped<IStorageProvider>(sp => new FileSystemStorageProvider(Path.Combine(Environment.CurrentDirectory, "chunks")));
        services.AddScoped<IStorageProvider, DatabaseStorageProvider>();
        services.AddScoped<StorageProviderFactory>();
        services.AddScoped<IFileRebuilder, FileRebuilder>();
        services.AddScoped<FileUploadService>();
    })
    .Build();


using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChunkDbContext>();
    db.Database.EnsureCreated();
}

await host.StartAsync();


var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("App");
logger.LogInformation("DistributedFileStorage started");

var uploadService = host.Services.GetRequiredService<FileUploadService>();
var rebuilder = host.Services.GetRequiredService<IFileRebuilder>();

while (true)
{
    Console.Write("Command (upload <path> | rebuild <fileId> <outputDir> | exit): ");
    string? input = Console.ReadLine();
    if (input == null)
    {
        continue;
    }

    var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length == 0)
    {
        continue;
    }

    switch (parts[0].ToLowerInvariant())
    {
        case "upload":
            if (parts.Length < 2)
            {
                Console.WriteLine("Path required.");
                break;
            }
            var path = parts[1];
            if (!File.Exists(path))
            {
                Console.WriteLine("File not found.");
                break;
            }
            var fileId = await uploadService.UploadAsync(path);
            Console.WriteLine($"Uploaded file id: {fileId}");
            break;

        case "rebuild":
            if (parts.Length < 3)
            {
                Console.WriteLine("Usage: rebuild <fileId> <outputDir>");
                break;
            }
            if (!Guid.TryParse(parts[1], out var rebuildId))
            {
                Console.WriteLine("Invalid file id.");
                break;
            }
            var outputDir = parts[2];
            Directory.CreateDirectory(outputDir);
            var outputPath = await rebuilder.RebuildAsync(rebuildId, outputDir);
            Console.WriteLine($"Rebuilt file at {outputPath}");
            break;

        case "exit":
            await host.StopAsync();
            return;

        default:
            Console.WriteLine("Unknown command.");
            break;
    }
}
