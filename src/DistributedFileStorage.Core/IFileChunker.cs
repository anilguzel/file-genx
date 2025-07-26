using System.Collections.Generic;

namespace DistributedFileStorage.Core;

public interface IFileChunker
{
    IEnumerable<FileChunk> ChunkFile(string filePath);
}
