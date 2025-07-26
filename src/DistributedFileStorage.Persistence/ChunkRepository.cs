using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DistributedFileStorage.Core;

namespace DistributedFileStorage.Persistence;

public class ChunkRepository : IChunkRepository
{
    private readonly ChunkDbContext _context;

    public ChunkRepository(ChunkDbContext context)
    {
        _context = context;
    }

    public async Task AddMetadataAsync(ChunkMetadata metadata)
    {
        _context.Metadata.Add(metadata);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ChunkMetadata>> GetChunksAsync(Guid fileId)
    {
        return await _context.Metadata
            .AsNoTracking()
            .Where(m => m.FileId == fileId)
            .OrderBy(m => m.ChunkIndex)
            .ToListAsync();
    }
}
