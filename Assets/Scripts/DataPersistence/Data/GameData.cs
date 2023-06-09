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
    public SerializableDictionary<string, CombinationLockData> CombinationLocks;
    #endregion

    #region PLOT
    public SerializableHashSet<string> AchievedPlotPoints;
    #endregion

    #region UI
    public SerializableDictionary<string, UIData> UIObjects;
    #endregion

    public GameData()
    {
      Player              = new PlayerData();
      Objects             = new SerializableDictionary<string, ObjectData>();
      UIObjects           = new SerializableDictionary<string, UIData>();
      DialogueAssetStates = new SerializableDictionary<string, string>();
      CombinationLocks    = new SerializableDictionary<string, CombinationLockData>();
      AchievedPlotPoints  = new SerializableHashSet<string>();
    }
  }
}
