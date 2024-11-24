using Zenject;

public class BootstrapSceneInstaller : MonoInstaller<BootstrapSceneInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBootstrapper>().To<Bootstrapper>().AsSingle().NonLazy();
    }
}