using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class SceneLoader : ISceneLoader
{
    //TODO: добавить остальные сцены
    private const string MainSceneKey = "MainMenu";
    

    public async UniTask LoadSceneAsync(string sceneName)
    {
        await Addressables.LoadSceneAsync(sceneName);
    }

    public async UniTask LoadMainSceneAsync()
    {
        await Addressables.LoadSceneAsync(MainSceneKey);
    }

   
}
