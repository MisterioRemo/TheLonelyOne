using System.Collections.Generic;
using UnityEngine;

namespace TheLonelyOne
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance { get; protected set; }

    #region PROPERTIES
    public Dictionary<string, GameObject> UIObjects { get; protected set; }
    #endregion

    protected void Awake()
    {
      if (Instance != null && Instance != this)
      {
        Destroy(this);
        return;
      }

      Instance = this;
    }

    protected void Start()
    {
      UIObjects = DecompositeUIsContainer();
    }

    protected Dictionary<string, GameObject> DecompositeUIsContainer()
    {
      Transform UIsContainer = GameObject.Find("UIs").transform;
      var       children     = new Dictionary<string, GameObject>();

      foreach (Transform child in UIsContainer)
        children.Add(child.name, child.gameObject);

      return children;
    }
  }
}
