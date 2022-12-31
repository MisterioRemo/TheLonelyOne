using System.Collections.Generic;
using UnityEngine;

namespace TheLonelyOne.SerializableTypes
{
  [System.Serializable]
  public class SerializableHashSet<T> : HashSet<T>, ISerializationCallbackReceiver
  {
    public List<T> values = new List<T>();
    public void OnAfterDeserialize()
    {
      Clear();
      foreach (T value in values)
        Add(value);
    }

    public void OnBeforeSerialize()
    {
      values.Clear();

      foreach (T value in this)
        values.Add(value);
    }
  }
}
