using System;
using System.Threading.Tasks;

namespace DistributedFileStorage.Core;

public interface IFileRebuilder
{
    Task<string> RebuildAsync(Guid fileId, string targetDirectory);
}
