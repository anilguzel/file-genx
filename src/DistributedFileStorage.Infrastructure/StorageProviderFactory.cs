using System;
using System.Collections.Generic;
using DistributedFileStorage.Core;

namespace DistributedFileStorage.Infrastructure;

public class StorageProviderFactory
{
    private readonly IDictionary<string, IStorageProvider> _providers;

    public StorageProviderFactory(IEnumerable<IStorageProvider> providers)
    {
        _providers = new Dictionary<string, IStorageProvider>();
        foreach (var provider in providers)
        {
            _providers[provider.Name] = provider;
        }
    }

    public IStorageProvider GetProvider(string name)
    {
        if (_providers.TryGetValue(name, out var provider))
            return provider;
        throw new InvalidOperationException($"Provider '{name}' not found");
    }
}
