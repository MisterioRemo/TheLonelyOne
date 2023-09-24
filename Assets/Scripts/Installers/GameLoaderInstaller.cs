using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class GameLoaderInstaller : MonoInstaller
  {
    [SerializeField] protected GameObject gameLoader;
    public override void InstallBindings()
    {
      Container
        .Bind<GameLoader>()
        .FromComponentInNewPrefab(gameLoader)
        .AsSingle()
        .NonLazy();
    }
  }
}
