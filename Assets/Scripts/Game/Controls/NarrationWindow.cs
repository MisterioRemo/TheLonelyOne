using UnityEngine;
using TMPro;
using System.Collections;

namespace TheLonelyOne.Dialogue
{
  public class NarrationWindow : MonoBehaviour
  {
    #region CONSTANTS
    private const float TYPING_DELAY = 0.05f;
    #endregion

    #region PARAMETERS
    private TextMeshProUGUI text;
    private IEnumerator     typingCoroutine;
    private string          typingSentence;
    private int             typingIndex;
    private bool            isClearDeferred = false;
    #endregion

    #region PROPERTIES
    public bool   IsTyping { get; private set; } = false;
    public string Text     { get => text.text;
                             set => text.text = value;
                           }
    #endregion

    public void Start()
    {
      text = GetComponentInChildren<TextMeshProUGUI>();
      gameObject.SetActive(false);

      if (text == null)
        Debug.LogError($"Can't find TextMeshProUGUI component in \"{gameObject.name}\".");
    }

    #region METHODS
    private IEnumerator TypeText(string _sentence, float _delay)
    {
      IsTyping    = true;
      typingIndex = 0;

      foreach (char c in _sentence)
      {
        Text += c;
        typingIndex++;
        yield return new WaitForSeconds(_delay);
      }

      IsTyping = false;
    }
    #endregion

    #region INTERFACE
    public void SetVisibility(bool _value)
    {
      Clear();
      gameObject.SetActive(_value);
    }

    public void DrawNarration(string _sentence)
    {
      StopTyping();

      if (isClearDeferred)
      {
        Clear();
        isClearDeferred = false;
      }

      typingSentence  = _sentence;
      typingCoroutine = TypeText(_sentence, TYPING_DELAY);
      StartCoroutine(typingCoroutine);
    }

    public void StopTyping()
    {
      if (!IsTyping)
        return;

      StopCoroutine(typingCoroutine);
      typingCoroutine = null;
      IsTyping        = false;
      Text            += typingSentence.Substring(typingIndex);
    }

    public void Clear()
    {
      if (IsTyping)
      {
        isClearDeferred = true;
        return;
      }

      Text = "";
    }
    #endregion
  }
}
