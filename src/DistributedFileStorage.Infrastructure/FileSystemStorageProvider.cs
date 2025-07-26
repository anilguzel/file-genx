using System;
using System.IO;
using System.Threading.Tasks;
using DistributedFileStorage.Core;

namespace DistributedFileStorage.Infrastructure;

public class FileSystemStorageProvider : IStorageProvider
{
    private readonly string _basePath;

    public FileSystemStorageProvider(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public string Name => "FileSystem";

    public Task StoreAsync(FileChunk chunk)
    {
        var path = Path.Combine(_basePath, chunk.Id.ToString());
        return File.WriteAllBytesAsync(path, chunk.Data);
    }

    public async Task<FileChunk?> RetrieveAsync(Guid chunkId)
    {
        var path = Path.Combine(_basePath, chunkId.ToString());
        if (!File.Exists(path)) return null;
        var data = await File.ReadAllBytesAsync(path);
        return new FileChunk { Id = chunkId, Data = data };
    }
}
