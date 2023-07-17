using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class GameLoader : MonoBehaviour
  {
    #region PARAMETERS
    [Inject] protected DiContainer diContainer;
    #endregion

    #region LIFECYCLE
    protected void Awake()
    {
      DiContainerRef.Container = diContainer;
    }
    #endregion
  }
}
