using System;
using UnityEngine;

namespace TheLonelyOne
{
  [HelpURL("https://stackoverflow.com/a/58446816")]
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class ShowIfAttribute : PropertyAttribute
  {
    public ActionOnConditionFail Action     { get; private set; }
    public ConditionOperator     Operator   { get; private set; }
    public string[]              Conditions { get; private set; }

    public ShowIfAttribute(ActionOnConditionFail action, ConditionOperator conditionOperator, params string[] conditions)
    {
      Action     = action;
      Operator   = conditionOperator;
      Conditions = conditions;
    }
  }
}
