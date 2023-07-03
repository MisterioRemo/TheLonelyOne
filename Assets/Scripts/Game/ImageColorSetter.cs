using UnityEngine;
using UnityEngine.UI;

namespace TheLonelyOne
{
  [RequireComponent(typeof(Image))]
  public class ImageColorSetter : MonoBehaviour, IColorSetter
  {
    #region PARAMETERS
    [SerializeField] protected LightColorPreset preset;

    protected Image[] images;
    #endregion

    #region ICOLORSETTER
    public void SetColor(float time)
    {
      if (!gameObject.activeInHierarchy || images == null)
        return;

      if (preset == null)
      {
        Debug.LogError($"LightColorPreset is not set for the object '{name}'.");
        return;
      }

      foreach(var image in images)
        image.color = preset.skyColor.Evaluate(time);
    }
    #endregion

    #region LIFECYCLE
    protected void Start()
    {
      images = GetComponentsInChildren<Image>();
    }
    #endregion
  }
}
