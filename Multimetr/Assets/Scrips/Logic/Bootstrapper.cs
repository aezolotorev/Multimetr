public class Bootstrapper : IBootstrapper
{
    private ISceneLoader _sceneLoader;
    
    public Bootstrapper(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
        Init();
    }

    public  void Init()
    {
        ///ининциализация всего что нужно
        LoadNextScene();
    }

    public  void LoadNextScene()
    {
        _sceneLoader.LoadMainSceneAsync();
    }
}