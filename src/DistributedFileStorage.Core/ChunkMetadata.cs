using System;

namespace DistributedFileStorage.Core;

public class ChunkMetadata
{
    public Guid FileId { get; set; }
    public Guid ChunkId { get; set; }
    public int ChunkIndex { get; set; }
    public string Checksum { get; set; } = string.Empty;
    public string StorageProviderName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
