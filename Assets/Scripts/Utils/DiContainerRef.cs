using Zenject;

namespace TheLonelyOne
{
  public static class DiContainerRef
  {
    private static DiContainer diContainer;

    public static DiContainer Container { get => diContainer;
                                          set => diContainer ??= value;
                                        }
  }
}
