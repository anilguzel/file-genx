using System;
using System.ComponentModel.DataAnnotations;

namespace DistributedFileStorage.Core;

public class ChunkMetadata
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FileId { get; set; }
    [Key]
    public Guid ChunkId { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public int ChunkIndex { get; set; }
    public string Checksum { get; set; } = string.Empty;
    public string StorageProviderName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
