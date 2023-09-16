using UnityEngine;

namespace TheLonelyOne.Dialogue
{
  public static class Utils
  {
    /// <summary>
    /// Return current variable value from Ink Story. Expensive operation.
    /// </summary>
    public static object GetInkVariableState(TextAsset _inkAsset, string _inkState, string _varName)
    {
      var story = new Ink.Runtime.Story(_inkAsset.text);

      if (!string.IsNullOrEmpty(_inkState))
        story.state.LoadJson(_inkState);

      return story.variablesState[_varName];
    }
  }
}
