using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheLonelyOne
{
  public class PlotManager : IDataPersistence
  {
    #region PARAMETERS
    private HashSet<string> achievedPlotPoints = new HashSet<string>();
    #endregion

    #region IDataPersistence
    public void Load(GameData _gameData)
    {
      achievedPlotPoints = _gameData.AchievedPlotPoints;
    }

    public void Save(ref GameData _gameData)
    {
      _gameData.AchievedPlotPoints = (SerializableTypes.SerializableHashSet<string>)achievedPlotPoints;
    }
    #endregion

    #region METHODS
    private List<Goal.PlotPoint> FindAllPlotPoints()
    {
      return UnityEngine.Object.FindObjectsOfType<Goal.PlotPoint>().ToList();
    }
    #endregion

    #region INTERFACE
    public bool IsPlotPointAchieved(string _name)
    {
      return achievedPlotPoints.Contains(_name);
    }

    public void AddAchievedPlotPoint(string _name)
    {
      if (!achievedPlotPoints.Add(_name))
        Debug.LogError("Tried to add an already existing achieved plot point.");
    }

    public Goal.PlotPoint FindPlotPoint(string _name)
    {
      return UnityEngine.Object.FindObjectsOfType<Goal.PlotPoint>().FirstOrDefault(x => x.Name == _name);
    }
    #endregion
  }
}
