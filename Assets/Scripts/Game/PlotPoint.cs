using UnityEngine;
using Zenject;

namespace TheLonelyOne.Goal
{
  public class PlotPoint : MonoBehaviour, IGoal
  {
    #region PARAMETERS
    [Inject] protected PlotManager plotManager;
    #endregion

    #region PROPERTIES
    [field: SerializeField]
    public virtual string Name { get; protected set; }
    #endregion

    #region IGOAL
    public virtual bool IsAchieved { get; protected set; }

    public virtual void Complete()
    {
      if (!IsAchieved)
      {
        plotManager.AddAchievedPlotPoint(Name);
        IsAchieved = true;
      }
    }

    #endregion

    #region LIFECYCLE
    public virtual void Start()
    {
      IsAchieved = plotManager.IsPlotPointAchieved(Name);
    }
    #endregion
  }
}
