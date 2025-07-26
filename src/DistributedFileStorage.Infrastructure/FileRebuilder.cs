using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DistributedFileStorage.Core;

namespace DistributedFileStorage.Infrastructure;

public class FileRebuilder : IFileRebuilder
{
    private readonly IChunkRepository _repository;
    private readonly StorageProviderFactory _factory;

    public FileRebuilder(IChunkRepository repository, StorageProviderFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<string> RebuildAsync(Guid fileId, string targetDirectory)
    {
        var metadata = (await _repository.GetChunksAsync(fileId)).OrderBy(c => c.ChunkIndex);
        var outputPath = Path.Combine(targetDirectory, fileId.ToString());
        await using var output = File.Create(outputPath);

        using var sha = SHA256.Create();
        foreach (var chunk in metadata)
        {
            var provider = _factory.GetProvider(chunk.StorageProviderName);
            var dataChunk = await provider.RetrieveAsync(chunk.ChunkId);
            if (dataChunk == null) throw new FileNotFoundException($"Chunk {chunk.ChunkId} not found");
            await output.WriteAsync(dataChunk.Data);
            sha.TransformBlock(dataChunk.Data, 0, dataChunk.Data.Length, null, 0);
        }
        sha.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        var checksum = Convert.ToHexString(sha.Hash!);

        var original = metadata.FirstOrDefault()?.Checksum;
        if (original != null && !original.Equals(checksum, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Checksum mismatch");

        return outputPath;
    }
}
