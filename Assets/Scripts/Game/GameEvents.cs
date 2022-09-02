using System;
using UnityEngine;

namespace TheLonelyOne
{
  public class GameEvents : MonoBehaviour
  {
    public static GameEvents Instance { get; private set; }

    private void Awake()
    {
      enabled = false;

      if (Instance != null && Instance != this)
      {
        Destroy(this);
        return;
      }

      Instance = this;
    }

    #region PLAYER
    public event Action OnPlayerMoving;

    public void PlayerMoving()
    {
      OnPlayerMoving?.Invoke();
    }
    #endregion
  }
}
