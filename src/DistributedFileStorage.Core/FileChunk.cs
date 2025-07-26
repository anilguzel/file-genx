using System;

namespace DistributedFileStorage.Core;

public class FileChunk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FileId { get; set; }
    public int ChunkIndex { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public string Checksum { get; set; } = string.Empty;
    public string StorageProviderName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
