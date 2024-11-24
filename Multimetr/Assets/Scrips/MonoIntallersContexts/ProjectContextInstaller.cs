using UnityEngine;
using Zenject;

public class ProjectContextInstaller : MonoInstaller<ProjectContextInstaller>
{
    public override void InstallBindings()
    {        
        Container.Bind<IAssetService>().To<AssetService>().AsSingle();
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
    }
}
