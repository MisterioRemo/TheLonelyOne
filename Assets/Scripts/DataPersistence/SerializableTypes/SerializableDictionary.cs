using System.Collections.Generic;
using UnityEngine;

namespace TheLonelyOne.SerializableTypes
{
  [System.Serializable]
  public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
  {
    public List<TKey>   keys   = new List<TKey>();
    public List<TValue> values = new List<TValue>();

    public void OnAfterDeserialize()
    {
      Clear();

      for (int i = 0; i < keys.Count; ++i)
        Add(keys[i], values[i]);
    }

    public void OnBeforeSerialize()
    {
      keys.Clear();
      values.Clear();

      foreach(var pair in this)
      {
        keys.Add(pair.Key);
        values.Add(pair.Value);
      }
    }
  }
}
