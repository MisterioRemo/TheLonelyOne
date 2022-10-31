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

    /// <summary>
    /// Определяет знак числа (для нуля возвращает 0).
    /// </summary>
    /// <param name="_value"></param>
    /// <returns></returns>
    public static int Sign(float _value)
    {
      return _value < 0 ? -1 : (_value > 0 ? 1 : 0);
    }

    public static MovementDirection GetMovementDirection(int _sign)
    {
      switch (_sign)
      {
        case 1:
          return MovementDirection.Right;
        case -1:
          return MovementDirection.Left;
        default:
          return MovementDirection.None;
      }
    }

    public static MovementDirection GetOppositeDirection(MovementDirection _direction)
    {
      switch (_direction)
      {
        case MovementDirection.Left:
          return MovementDirection.Right;

        case MovementDirection.Right:
          return MovementDirection.Left;

        case MovementDirection.None:
          return MovementDirection.Horizontal;

        case MovementDirection.Horizontal:
        default:
          return MovementDirection.None;
      }
    }

    /// <summary>
    /// Генерация уникального id.
    /// </summary>
    /// <returns></returns>
    public static string GenerateGuid()
    {
      return System.Guid.NewGuid().ToString();
    }
  }
}
