using UnityEngine;

namespace TheLonelyOne
{
  public class GameLoader : MonoBehaviour
  {
    #region CONTROLLERS
    public GameObject SaveSystem;
    public GameObject GameEvents;
    public GameObject DialogueManager;
    #endregion

    private void Awake()
    {
      enabled = false;

      if (TheLonelyOne.SaveSystem.Instance == null)
        Instantiate(SaveSystem);

      if (TheLonelyOne.GameEvents.Instance == null)
        Instantiate(GameEvents);

      if (Dialogue.DialogueManager.Instance == null)
        Instantiate(DialogueManager);

      // TEMP!
      TheLonelyOne.SaveSystem.Instance.NewGame();
    }
  }
}
