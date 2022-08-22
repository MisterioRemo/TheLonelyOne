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

      private static GameObject ParseFuctionTarget(string _targetName)
      {
        string[] hierarchy     = _targetName.Split('/', 2);
        string participantName = hierarchy[0].ToLower();

        if (Instance.participants.ContainsKey(participantName))
        {
          return hierarchy.Length > 1
                 ? Instance.participants[participantName].transform.root.Find(hierarchy[1]).gameObject
                 : Instance.participants[participantName].transform.root.gameObject;
        }

        return GameObject.Find(_targetName);
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
