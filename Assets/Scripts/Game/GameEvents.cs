using System;
using System.Collections.Generic;
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
    public event Action          OnPlayerMoving;
    public event Action<Vector3> OnPlayerTeleporting;

    public void PlayerMoving()
    {
      OnPlayerMoving?.Invoke();
    }

    public void PlayerTeleporting(Vector3 _position)
    {
      OnPlayerTeleporting?.Invoke(_position);
    }
    #endregion

    #region PUZZLES
    public event Action<string> OnPuzzleCompleted;

    public void CompletePuzzle(string _puzzleId)
    {
      OnPuzzleCompleted?.Invoke(_puzzleId);
    }
    #endregion
  }
}
