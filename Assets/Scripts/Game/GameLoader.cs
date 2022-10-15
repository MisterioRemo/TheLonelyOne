using UnityEngine;

namespace TheLonelyOne
{
  public class GameLoader : MonoBehaviour
  {
    #region CONTROLLERS
    public GameObject GameManager;
    public GameObject SaveSystem;
    public GameObject GameEvents;
    #endregion

    private void Awake()
    {
      enabled = false;

      if (TheLonelyOne.GameManager.Instance == null)
        Instantiate(GameManager);

      if (TheLonelyOne.SaveSystem.Instance == null)
        Instantiate(SaveSystem);

      if (TheLonelyOne.GameEvents.Instance == null)
        Instantiate(GameEvents);
    }
  }
}
