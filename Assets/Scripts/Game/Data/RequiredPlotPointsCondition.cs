using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  [Serializable]
  public class RequiredPlotPointsCondition : PlotPointList, IObjectWithConditions
  {
    #region PARAMETERS
    [Inject] protected PlotManager plotManager;
    #endregion

    #region IObjectWithConditions
    [field: SerializeField]
    public ConditionOperator Operator { get; protected set; }

    public bool IsAchieved => Operator == ConditionOperator.Or
                                ? plotPoints.Any(point => point.IsAchieved) ||
                                  plotPointNames.Any(pointName => plotManager.IsPlotPointAchieved(pointName))
                                : plotPoints.All(point => point.IsAchieved) &&
                                  plotPointNames.All(pointName => plotManager.IsPlotPointAchieved(pointName));
    #endregion


    #region INTERFACE
    public void Copy(RequiredPlotPointsCondition _fromList)
    {
      base.Copy(_fromList);
      Operator = _fromList.Operator;
    }
    #endregion
  }
}
