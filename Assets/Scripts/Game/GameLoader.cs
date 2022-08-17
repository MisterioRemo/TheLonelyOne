using UnityEngine;

namespace TheLonelyOne
{
  public class GameLoader : MonoBehaviour
  {
    #region CONTROLLERS
    public GameObject EventsManager;
    public GameObject DialogueManager;
    #endregion

    private void Awake()
    {
      enabled = false;

      if (GameEvents.Instance == null)
        Instantiate(EventsManager);

      if (Dialogue.DialogueManager.Instance == null)
        Instantiate(DialogueManager);
    }
  }
}
