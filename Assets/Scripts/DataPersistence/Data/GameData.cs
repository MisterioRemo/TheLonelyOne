using TheLonelyOne.SerializableTypes;

namespace TheLonelyOne
{
  [System.Serializable]
  public class GameData
  {
    #region DIALOGUE
    public SerializableDictionary<string, string> DialogueAssetStates;
    #endregion

    public GameData()
    {
      DialogueAssetStates = new SerializableDictionary<string, string>();
    }
  }
}
