namespace TheLonelyOne.Goal
{
  public interface IGoal
  {
    bool IsAchieved { get; }
    void Complete();
  }
}
