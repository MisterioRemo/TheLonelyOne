using TheLonelyOne.SerializableTypes;

namespace TheLonelyOne
{
  [System.Serializable]
  public class GameData
  {
    #region OBJECTS
    public PlayerData                                 Player;
    public SerializableDictionary<string, ObjectData> Objects;
    #endregion

    #region DIALOGUE
    public SerializableDictionary<string, string> DialogueAssetStates;
    #endregion

    #region PUZZLES
    public SerializableDictionary<string, string> CombinationLocks;
    #endregion

    #region PLOT
    public SerializableHashSet<string> AchievedPlotPoints;
    #endregion

    public GameData()
    {
      Player              = new PlayerData();
      Objects             = new SerializableDictionary<string, ObjectData>();
      DialogueAssetStates = new SerializableDictionary<string, string>();
      CombinationLocks    = new SerializableDictionary<string, string>();
      AchievedPlotPoints  = new SerializableHashSet<string>();
    }
  }
}
