using System;

namespace TheLonelyOne
{
  public interface IEndEvent
  {
    event Action OnEnded;
  }
}
