using System;
using System.Collections.Generic;

namespace TheLonelyOne
{
  [Serializable]
  public class PlotPointList
  {

    #region PARAMETERS
    public List<Goal.PlotPoint> plotPoints;
    public List<string>         plotPointNames;
    #endregion

    #region INTERFACE
    public void Copy(PlotPointList _fromList)
    {
      plotPoints     = _fromList.plotPoints;
      plotPointNames = _fromList.plotPointNames;
    }
    #endregion
  }
}
