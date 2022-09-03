using UnityEngine;

namespace TheLonelyOne
{
  /// <summary>
  /// Attribute that require implementation of the provided interface.
  /// </summary>
  public class RequireInterfaceAttribute : PropertyAttribute
  {
    public System.Type requiredType { get; private set; }

    public RequireInterfaceAttribute(System.Type type)
    {
      requiredType = type;
    }
  }
}