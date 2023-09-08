using UnityEngine;
using TMPro;

namespace TheLonelyOne.Dialogue
{
  public class NarrationWindow : MonoBehaviour
  {
    #region PARAMETERS
    private TextMeshProUGUI text;
    #endregion

    public string Text
    {
      get => text.text;
      set => text.text = value;
    }

    public void Start()
    {
      text = GetComponentInChildren<TextMeshProUGUI>();
      gameObject.SetActive(false);

      if (text == null)
        Debug.LogError($"Can't find TextMeshProUGUI component in \"{gameObject.name}\".");
    }

    #region METHODS
    #endregion

    #region INTERFACE
    public void SetVisibility(bool _value)
    {
      Clear();
      gameObject.SetActive(_value);
    }

    public void DrawNarration(string _sentence)
    {
      text.text += _sentence;
    }

    public void Clear()
    {
      text.text = "";
    }
    #endregion
  }
}
