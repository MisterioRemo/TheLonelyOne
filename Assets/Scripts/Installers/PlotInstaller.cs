using Zenject;

namespace TheLonelyOne
{
  public class PlotInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container
        .BindInterfacesAndSelfTo<PlotManager>()
        .FromNew()
        .AsSingle()
        .NonLazy();
    }
  }
}
