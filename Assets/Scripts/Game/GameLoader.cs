using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class GameLoader : MonoBehaviour
  {
    [Inject] protected DiContainer diContainer;

    private void Awake()
    {
      enabled = false;
      DiContainerRef.Container = diContainer;
    }
  }
}
