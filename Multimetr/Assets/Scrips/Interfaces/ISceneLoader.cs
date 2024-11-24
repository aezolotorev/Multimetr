using Cysharp.Threading.Tasks;


public interface ISceneLoader
{

    UniTask LoadSceneAsync(string sceneName);

    UniTask LoadMainSceneAsync();

    
}
