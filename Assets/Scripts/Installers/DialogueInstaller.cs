using Zenject;

namespace TheLonelyOne
{
  public class DialogueInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container
        .BindInterfacesAndSelfTo<Dialogue.DialogueManager>()
        .FromNew()
        .AsSingle()
        .NonLazy();

      Container
        .Bind<Dialogue.DialogueAction>()
        .FromNew()
        .AsSingle();
    }
  }
}
