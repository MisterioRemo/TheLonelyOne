using UnityEngine;

namespace TheLonelyOne
{
  public class SpeakableObjectWithConditions : SpeakableObject
  {
    #region PARAMETERS
    [SerializeField] private RequiredPlotPointsCondition requiredPlotPoints;
    #endregion

    #region InteractableObject
    public override void Interact()
    {
      if (IconVisability)
        base.Interact();
    }
    #endregion

    #region METHODS
    protected override bool ShouldDisplayIcon()
    {
      return requiredPlotPoints.IsAchieved && base.ShouldDisplayIcon();
    }
    #endregion

    #region LIFECYCLE
    protected virtual void Start()
    {
      // т.к. нужно иметь возможность установить requiredPlotPoints из эдитора,
      // но также иметь доступ к PlotManager через Inject
      var points = DiContainerRef.Container.Instantiate<RequiredPlotPointsCondition>();
      points.Copy(requiredPlotPoints);
      requiredPlotPoints = points;
    }
    #endregion
  }
}
