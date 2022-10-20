using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class PersistentDataInstaller : MonoInstaller
  {
    [SerializeField] protected GameObject saveSystemPrefab;
    [SerializeField] protected string     fileName;
    [SerializeField] protected bool       useEncryption;

    public override void InstallBindings()
    {
      Container
       .Bind<SaveSystem>()
       .FromComponentInNewPrefab(saveSystemPrefab)
       .AsSingle()
       .NonLazy();

      Container.BindInstance(fileName).WhenInjectedInto<SaveSystem>();
      Container.BindInstance(useEncryption).WhenInjectedInto<SaveSystem>();
    }
  }
}