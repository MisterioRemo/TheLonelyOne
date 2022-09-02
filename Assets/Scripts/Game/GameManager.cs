using System.Collections.Generic;
using UnityEngine;

namespace TheLonelyOne
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance { get; private set; }

    #region PROPERTIES
    public Dictionary<string, GameObject> UIObjects { get; private set; }
    #endregion

    private void Awake()
    {
      if (Instance != null && Instance != this)
      {
        Destroy(this);
        return;
      }

      Instance = this;
    }

    private void Start()
    {
      UIObjects = DecompositeUIsContainer();
    }

    private Dictionary<string, GameObject> DecompositeUIsContainer()
    {
      Transform UIsContainer = GameObject.Find("UIs").transform;
      var       children     = new Dictionary<string, GameObject>();

      foreach (Transform child in UIsContainer)
        children.Add(child.name, child.gameObject);

      return children;
    }
  }
}
