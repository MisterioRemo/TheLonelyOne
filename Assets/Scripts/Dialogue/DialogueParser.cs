using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheLonelyOne.Dialogue
{
  public partial class DialogueManager
  {
    public class DialogueParser
    {
      #region PARAMETERS
      protected readonly DialogueManager parent;
      #endregion

      public DialogueParser(DialogueManager _dialogueManager)
      {
        parent = _dialogueManager;
      }

      #region FUNCTIONS
      internal bool ParseFunction(string _text, out string _functionName, out string[] _params)
      {
        if (!_text.StartsWith('>'))
        {
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
        _params       = functionData[1..];
        return true;
      }

      public GameObject ParseFuctionTarget(string _targetName)
      {
        string[] hierarchy     = _targetName.Split('/', 2);
        string participantName = hierarchy[0].ToLower();

        if (parent.participants.ContainsKey(participantName))
        {
          return hierarchy.Length > 1
                 ? parent.participants[participantName].transform.root.Find(hierarchy[1]).gameObject
                 : parent.participants[participantName].transform.root.gameObject;
        }

        return GameObject.Find(_targetName);
      }
      #endregion

      #region TAGS
      public List<string> GetCustomTags(string _text)
      {
        return _text
                .Split(' ')
                .Where(x => x.StartsWith('$'))
                .Select(x => x[1..])
                .ToList();
      }

      public void ParseTags(List<string> _tags)
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

      private void ParseSingleTag(string _tag)
      {
        switch (_tag)
        {
          default:
            return;
        }
      }

      private void ParseParameterizedTag(string[] _tags)
      {
        switch (_tags[0])
        {
          case "speaker":
            parent.currentParticipantName = _tags[1];
            return;

          default:
            return;
        }
      }
    }
    #endregion
  }
}
