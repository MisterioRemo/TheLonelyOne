using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class UIElementInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container
        .BindInterfacesAndSelfTo<UIObjectsManager>()
        .FromNew()
        .AsSingle()
        .NonLazy();
    }
  }
}