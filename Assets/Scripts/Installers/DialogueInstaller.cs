using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class DialogueInstaller : MonoInstaller
  {
    [SerializeField] protected GameObject narrationWindowUIPrefab;
    public override void InstallBindings()
    {
      Container
        .BindInterfacesAndSelfTo<Dialogue.DialogueManager>()
        .FromNew()
        .AsSingle()
        .NonLazy();

      Container.BindInstance(narrationWindowUIPrefab).WhenInjectedInto<Dialogue.DialogueManager>();
    }
  }
}
