using System;
using System.Threading.Tasks;
using DistributedFileStorage.Core;
using DistributedFileStorage.Persistence;

namespace DistributedFileStorage.Infrastructure;

public class DatabaseStorageProvider : IStorageProvider
{
    private readonly ChunkDbContext _context;

    public DatabaseStorageProvider(ChunkDbContext context)
    {
        _context = context;
    }

    public string Name => "Database";

    public async Task StoreAsync(FileChunk chunk)
    {
        _context.Chunks.Add(chunk);
        await _context.SaveChangesAsync();
    }

    public Task<FileChunk?> RetrieveAsync(Guid chunkId)
    {
        return _context.Chunks.FindAsync(chunkId).AsTask();
    }
}
