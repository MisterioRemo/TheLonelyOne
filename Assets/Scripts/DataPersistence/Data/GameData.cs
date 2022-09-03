using TheLonelyOne.SerializableTypes;

namespace TheLonelyOne
{
  [System.Serializable]
  public class GameData
  {
    #region DIALOGUE
    public SerializableDictionary<string, string> DialogueAssetStates;
    #endregion

    #region PUZZLES
    public SerializableDictionary<string, string> CombinationLocks;
    #endregion

    public GameData()
    {
      DialogueAssetStates = new SerializableDictionary<string, string>();
      CombinationLocks    = new SerializableDictionary<string, string>();
    }
  }
}
