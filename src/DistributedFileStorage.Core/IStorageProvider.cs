using System;
using System.Threading.Tasks;

namespace DistributedFileStorage.Core;

public interface IStorageProvider
{
    string Name { get; }
    Task StoreAsync(FileChunk chunk);
    Task<FileChunk?> RetrieveAsync(Guid chunkId);
}
