using TheLonelyOne.SerializableTypes;
using UnityEngine;

namespace TheLonelyOne
{
  [System.Serializable]
  public class ObjectData
  {
    public Vector3 Position;
    public bool    IsActive;

    [SerializeReference] public ObjectStateData              State;
    [SerializeReference] public SerializableObjectDictionary Children;
  }
}
