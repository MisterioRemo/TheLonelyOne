using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheLonelyOne.Dialogue
{
  public partial class DialogueManager
  {
    private static class Parser
    {
      #region FUNCTIONS
      public static bool ParseFunction(string _text, out GameObject _target, out string _functionName, out string[] _params)
      {
        if (!_text.StartsWith('>'))
        {
          _target       = null;
          _functionName = null;
          _params       = null;
          return false;
        }

        string[] functionData = _text
                                 .Substring(1)
                                 .Trim('\r', '\n')
                                 .Split(' ')
                                 .Where(x => !string.IsNullOrEmpty(x))
                                 .ToArray();

        _functionName = functionData[0];
        _target       = functionData.Length > 1 ? ParseFuctionTarget(functionData[1]) : null;
        _params       = functionData.Length > 2 ? functionData[2..] : null;
        return true;
      }

      private static GameObject ParseFuctionTarget(string _targetData)
      {
        string[] properties = _targetData.Split('.');
        GameObject target   = Instance.participants[properties[0].ToLower()].transform.root.gameObject;

        if (properties.Length == 1)
          return target;

        int index = properties[1].IndexOf('[');

        if (index == -1)
          return target.GetComponent(properties[1]).gameObject;

        string componentName = properties[1].Substring(0, index);
        string childName     = properties[1].Substring(index + 1, properties[1].Length - index - 2);

        return target.transform.Find(childName).GetComponent(componentName).gameObject;
      }
      #endregion

      #region TAGS
      public static List<string> GetCustomTags(string _text)
      {
        return _text
                .Split(' ')
                .Where(x => x.StartsWith('$'))
                .Select(x => x.Substring(1))
                .ToList();
      }

      public static void ParseTags(List<string> _tags)
      {
        if (_tags == null)
          return;

        foreach (var tag in _tags)
        {
          string[] values = tag.ToLower().Split(":");

          if (values.Length == 1)
            ParseSingleTag(values[0]);
          else
            ParseParameterizedTag(values);
        }
      }

      private static void ParseSingleTag(string _tag)
      {
        switch (_tag)
        {
          default:
            return;
        }
      }

      private static void ParseParameterizedTag(string[] _tags)
      {
        switch (_tags[0])
        {
          case "speaker":
            Instance.currentParticipantName = _tags[1];
            return;

          default:
            return;
        }
      }
    }
    #endregion
  }
}
