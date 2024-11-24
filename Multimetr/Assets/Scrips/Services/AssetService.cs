using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;


public class AssetService : IAssetService, IDisposable
{
    private readonly List<object> _cache = new();

    public async UniTask<TAsset> GetAssetAsync<TAsset>(string addressableKey) where TAsset : class
    {
        var result = await LoadNewAssetAsync<TAsset>(addressableKey);

        return result;
    }

    private async UniTask<TAsset> LoadNewAssetAsync<TAsset>(string addressableKey) where TAsset : class
    {
        var asyncOperationHandle = Addressables.LoadAssetAsync<TAsset>(addressableKey);

        _cache.Add(asyncOperationHandle);

        var result = await asyncOperationHandle;

        return result;
    }

    private void ReleaseAsset()
    {
        foreach (var handle in _cache)
        {
            Addressables.Release(handle);
        }

        _cache.Clear();
    }

    public void Dispose()
    {
        ReleaseAsset();
    }
}
