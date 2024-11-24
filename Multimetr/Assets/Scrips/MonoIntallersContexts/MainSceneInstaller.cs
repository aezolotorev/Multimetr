using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller<MainSceneInstaller>
{
    [SerializeField] SceneObjectContainer _sceneObjectContainer;
    public override void InstallBindings()
    {
        Container.Bind<InputController>().AsSingle().NonLazy();
        Container.Bind<ISceneObjectContainer>().FromInstance(_sceneObjectContainer).AsSingle();
        Container.Bind<MultiMetr>().AsSingle();
        Container.Bind<MultiMetrInteractor>().AsSingle().NonLazy();
    }
}