namespace TheLonelyOne
{
  public interface IObjectWithConditions
  {
    ConditionOperator Operator   { get; }
    bool              IsAchieved { get; }
  }
}
