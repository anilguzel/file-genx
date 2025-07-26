using Microsoft.EntityFrameworkCore;
using DistributedFileStorage.Core;

namespace DistributedFileStorage.Persistence;

public class ChunkDbContext : DbContext
{
    public ChunkDbContext(DbContextOptions<ChunkDbContext> options) : base(options) { }

    public DbSet<FileChunk> Chunks => Set<FileChunk>();
    public DbSet<ChunkMetadata> Metadata => Set<ChunkMetadata>();
}
