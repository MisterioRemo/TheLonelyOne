using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TheLonelyOne
{
  [RequireComponent(typeof(Light2D))]
  public class LightColorSetter : MonoBehaviour, IColorSetter
  {
    #region PARAMETERS
    [SerializeField] protected LightColorPreset preset;

    protected Light2D light2D;
    #endregion

    #region ICOLORSETTER
    public void SetColor(float time)
    {
      if (preset == null)
      {
        Debug.LogError($"LightColorPreset is not set for the object '{name}'.");
        return;
      }

      light2D.color = preset.skyColor.Evaluate(time);
    }
    #endregion

    #region LIFECYCLE
    protected void Awake()
    {
      light2D = GetComponent<Light2D>();
    }
    #endregion
  }
}
