using UnityEngine;

namespace TheLonelyOne
{
  [System.Serializable]
  public class UIData
  {
    public bool IsActive;
    [SerializeReference] public UIStateData State;
  }
}
