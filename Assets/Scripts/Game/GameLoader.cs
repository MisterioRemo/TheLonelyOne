using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class GameLoader : MonoBehaviour
  {
    #region CONTROLLERS
    public GameObject GameEvents;
    #endregion

    [Inject] protected DiContainer diContainer;

    private void Awake()
    {
      enabled = false;

      if (TheLonelyOne.GameEvents.Instance == null)
        Instantiate(GameEvents);

      DiContainerRef.Container = diContainer;
    }
  }
}
