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
        var metadataList = (await _repository.GetChunksAsync(fileId))
            .OrderBy(c => c.ChunkIndex)
            .ToList();
        if (!metadataList.Any())
            throw new InvalidOperationException($"No chunks found for file {fileId}");
        var originalName = metadataList.First().OriginalFileName;
        var extension = Path.GetExtension(originalName);
        var fileName = string.IsNullOrWhiteSpace(extension)
            ? fileId.ToString()
            : $"{fileId}{extension}";
        var outputPath = Path.Combine(targetDirectory, fileName);
        await using var output = File.Create(outputPath);

        foreach (var chunk in metadataList)
        {
            var provider = _factory.GetProvider(chunk.StorageProviderName);
            var dataChunk = await provider.RetrieveAsync(chunk.ChunkId);
            if (dataChunk == null)
                throw new FileNotFoundException($"Chunk {chunk.ChunkId} not found");

            var computed = Convert.ToHexString(SHA256.HashData(dataChunk.Data));
            if (!chunk.Checksum.Equals(computed, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Checksum mismatch for chunk {chunk.ChunkId}");

            await output.WriteAsync(dataChunk.Data);
        }

        return outputPath;
    }
}
