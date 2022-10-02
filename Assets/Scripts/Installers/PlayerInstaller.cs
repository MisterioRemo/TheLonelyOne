using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class PlayerInstaller : MonoInstaller
  {
    [SerializeField] protected GameObject         player;
    [SerializeField] protected MovementParameters movementParameters;

    public override void InstallBindings()
    {
      Container
        .Bind<Player.PlayerController>()
        .FromNewComponentOn(player)
        .AsSingle()
        .NonLazy();

      Container
        .Bind<Player.PlayerMovementController>()
        .FromNewComponentOn(player)
        .AsSingle()
        .OnInstantiated<Player.PlayerMovementController>
          ((context, movementCtrl) => movementCtrl.Parameters = movementParameters)
        .NonLazy();

      Container
        .Bind<PlayerInputActions>()
        .FromNew()
        .AsSingle()
        .OnInstantiated<PlayerInputActions>
          ((context, inputActions) => inputActions.Player.Enable())
        .NonLazy();
    }
  }
}