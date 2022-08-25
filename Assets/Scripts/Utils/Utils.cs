using System.Globalization;
using System.Linq;
using UnityEngine;

namespace TheLonelyOne
{
  public static class Utils
  {
    /// <summary>
    /// Перевод строки в UnityEngine.Vector3.
    /// </summary>
    /// <param name="_value">строка может иметь вид "Vector3(0.0, 0.0, 0.0)", или "Vector(0.0, 0.0, 0.0)", или "(0.0, 0.0, 0.0)", или "0.0, 0.0, 0.0".</param>
    /// <returns></returns>
    public static Vector3 ParseToVector3(string _value)
    {
      var values = _value
                    .Substring(_value.IndexOf("(") + 1)
                    .TrimEnd(')', ' ')
                    .Split(',', ' ')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => float.Parse(x, CultureInfo.InvariantCulture))
                    .ToArray();

      return new Vector3(values[0], values[1], values[2]);
    }
  }
}
