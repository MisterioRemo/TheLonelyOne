using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class LightIntaller : MonoInstaller
  {
    [SerializeField] protected float startTime;
    [SerializeField] protected bool  setPauseTime;

    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(setPauseTime))]
    [SerializeField] protected float pauseTime;

    public override void InstallBindings()
    {
      var ctrl = setPauseTime
                 ? new LightColorController(startTime, pauseTime)
                 : new LightColorController(startTime);
      Container
        .BindInterfacesAndSelfTo<LightColorController>()
        .FromInstance(ctrl)
        .AsSingle()
        .NonLazy();

      Container.QueueForInject(ctrl);
    }
  }
}
