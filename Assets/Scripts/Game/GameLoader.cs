using UnityEngine;

namespace TheLonelyOne
{
  public class GameLoader : MonoBehaviour
  {
    #region CONTROLLERS
    public GameObject EventsManager;
    #endregion

    private void Awake()
    {
      enabled = false;

      if (GameEvents.Instance == null)
        Instantiate(EventsManager);
    }
  }
}
