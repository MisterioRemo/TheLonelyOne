namespace TheLonelyOne.Goal
{
  public class OpenDoorPlotPoint : PlotPoint, IReversible
  {
    #region PARAMETERS
    protected InteractableObject interactableObject;
    #endregion

    #region IREVERSIBLE
    public void Undo()
    {
      plotManager.RemoveAchievedPlotPoint(Name);
      IsAchieved = false;
    }
    #endregion

    #region LIFECYCLE
    public virtual void Awake()
    {
      interactableObject          = GetComponent<InteractableObject>();
      interactableObject.OnEnded += OnInteractionEnded;
    }
    #endregion

    #region METHODS
    private void OnInteractionEnded()
    {
      if (IsAchieved)
      {
        Undo();
        return;
      }

      Complete();
    }
    #endregion
  }
}
