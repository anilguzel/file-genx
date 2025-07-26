using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistributedFileStorage.Core;

public interface IChunkRepository
{
    Task AddMetadataAsync(ChunkMetadata metadata);
    Task<IEnumerable<ChunkMetadata>> GetChunksAsync(Guid fileId);
}
