using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
    public class UIObjectsManager : IInitializable
  {
    #region PROPERTIES
    public Dictionary<string, GameObject> UIObjects { get; protected set; }
    #endregion

    #region IInitializable
    public void Initialize()
    {
      UIObjects = DecompositeUIsContainer();
    }
    #endregion

    #region METHODS
    protected Dictionary<string, GameObject> DecompositeUIsContainer()
    {
      Transform UIsContainer = GameObject.Find("UIs").transform;
      var       children     = new Dictionary<string, GameObject>();

      foreach (Transform child in UIsContainer)
        children.Add(child.name, child.gameObject);

      return children;
    }
    #endregion
  }
}
