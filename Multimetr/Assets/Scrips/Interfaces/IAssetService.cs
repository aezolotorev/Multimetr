using Cysharp.Threading.Tasks;

public interface IAssetService
{
    public UniTask<TAsset> GetAssetAsync<TAsset>(string addressableKey) where TAsset : class;

}
