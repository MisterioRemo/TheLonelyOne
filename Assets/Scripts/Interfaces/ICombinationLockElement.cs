using System;

namespace TheLonelyOne
{
  public interface ICombinationLockElement
  {
    void GoToNextSymbol();
    void GoToPreviousSymbol();

    event Action OnSymbolChange;

    char Symbol { get; set; }
  }
}
