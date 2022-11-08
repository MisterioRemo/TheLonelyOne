using UnityEngine;

namespace TheLonelyOne
{
  [CreateAssetMenu(fileName = "LightColorPreset", menuName = "TheLonelyOne/LightColorPreset")]
  public class LightColorPreset : ScriptableObject
  {
    public Gradient skyColor;
    // public Gradient fogColor;
  }
}
