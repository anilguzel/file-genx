using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DistributedFileStorage.Core;

namespace DistributedFileStorage.Infrastructure;

public class FileUploadService
{
    private readonly IFileChunker _chunker;
    private readonly StorageProviderFactory _factory;
    private readonly IChunkRepository _repository;
    private readonly ILogger<FileUploadService> _logger;

    public FileUploadService(IFileChunker chunker, StorageProviderFactory factory, IChunkRepository repository, ILogger<FileUploadService> logger)
    {
        _chunker = chunker;
        _factory = factory;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> UploadAsync(string filePath)
    {
        Guid fileId = Guid.Empty;
        foreach (var chunk in _chunker.ChunkFile(filePath))
        {
            if (fileId == Guid.Empty) fileId = chunk.FileId;
            var provider = _factory.GetProvider("FileSystem");
            await provider.StoreAsync(chunk);
            await _repository.AddMetadataAsync(new ChunkMetadata
            {
                FileId = chunk.FileId,
                ChunkId = chunk.Id,
                OriginalFileName = chunk.OriginalFileName,
                ChunkIndex = chunk.ChunkIndex,
                Checksum = chunk.Checksum,
                StorageProviderName = provider.Name,
                CreatedAt = chunk.CreatedAt
            });
            _logger.LogInformation("Stored chunk {Chunk}", chunk.Id);
        }
        return fileId;
    }
}
