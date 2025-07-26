using System;
using System.IO;
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
        services.AddDbContext<ChunkDbContext>(opt => opt.UseInMemoryDatabase("chunks"));
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

var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("App");
logger.LogInformation("DistributedFileStorage started");

logger.LogInformation("This is a placeholder application - functionality is limited in this environment.");

await host.RunAsync();
