using System.Collections;
using System.Collections.Generic;

namespace TheLonelyOne
{
  public class ObjectArrayComparer<T> : IEqualityComparer<T[]>
  {
    public bool Equals(T[] x, T[] y)
    {
      return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
    }

    public int GetHashCode(T[] obj)
    {
      return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
    }
  }
}
