using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using DistributedFileStorage.Core;

namespace DistributedFileStorage.Infrastructure;

public class DynamicFileChunker : IFileChunker
{
    public IEnumerable<FileChunk> ChunkFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        var fileId = Guid.NewGuid();
        var fileInfo = new FileInfo(filePath);
        var chunkSize = GetChunkSize(fileInfo.Length);
        var buffer = new byte[chunkSize];
        var index = 0;
        using var stream = File.OpenRead(filePath);
        int read;
        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var data = new byte[read];
            Array.Copy(buffer, data, read);
            yield return new FileChunk
            {
                FileId = fileId,
                ChunkIndex = index++,
                OriginalFileName = fileInfo.Name,
                Data = data,
                Checksum = Convert.ToHexString(SHA256.HashData(data))
            };
        }
    }

    private static int GetChunkSize(long length)
    {
        if (length < 10 * 1024 * 1024) return 1 * 1024 * 1024;
        if (length < 100 * 1024 * 1024) return 5 * 1024 * 1024;
        return 10 * 1024 * 1024;
    }
}
