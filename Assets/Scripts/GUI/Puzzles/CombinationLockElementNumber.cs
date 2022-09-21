using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace TheLonelyOne.UI
{
  public class CombinationLockElementNumber : MonoBehaviour, ICombinationLockElement
  {
    #region PARAMETERS
    [SerializeField] protected Button          nextButton;
    [SerializeField] protected Button          previousButton;
    [SerializeField] protected TextMeshProUGUI symbolContainer;

    public readonly int MinValue = 0;
    public readonly int MaxValue = 9;

    protected int number;

    #endregion

    #region PROPERTIES
    public int Number { get => number;
                        set
                        {
                          number = (value < MinValue)
                                   ? MaxValue
                                   : (value > MaxValue) ? MinValue : value;
                          symbolContainer.text = number.ToString();
                        }
                      }
    #endregion

    #region ICombinationLockElement
    public event Action OnSymbolChange;

    public char Symbol { get => (char)(Number + 48);
                         set => Number = Mathf.Max('0', Mathf.Min('9', value)) - 48;
                       }

    public void GoToNextSymbol()
    {
      Number++;
      ChangeSymbol();
    }

    public void GoToPreviousSymbol()
    {
      Number--;
      ChangeSymbol();
    }
    #endregion

    protected void ChangeSymbol()
    {
      OnSymbolChange?.Invoke();
    }
  }
}
