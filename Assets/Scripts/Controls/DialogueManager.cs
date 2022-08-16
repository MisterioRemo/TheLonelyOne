using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TheLonelyOne.Dialogue
{
  public class DialogueManager : MonoBehaviour
  {
    public static DialogueManager Instance { get; private set; }

    #region PARAMETERS
    private Story                                   inkStory;
    private string                                  currentParticipantName;
    private int                                     currentChoiceIndex;
    private bool                                    isChoiceIndexSelected;
    private Dictionary<string, DialogueParticipant> participants;
    #endregion

    #region PROPERTIES
    public bool IsDialoguePlaying { get; private set; }
    private int CurrentChoiceIndex { get => currentChoiceIndex;
                                     set {
                                       currentChoiceIndex = Mathf.Max(0, value);
                                       if (inkStory && inkStory.currentChoices.Count > 0)
                                         currentChoiceIndex %= inkStory.currentChoices.Count;
                                     }
                                   }
    #endregion

    private void Awake()
    {
      enabled = false;

      if (Instance != null && Instance != this)
      {
        Destroy(this);
        return;
      }

      Instance     = this;
      participants = new Dictionary<string, DialogueParticipant>();
    }

    private void Start()
    {
      ResetParameters();
    }

    private void ResetParameters()
    {
      currentParticipantName = "";
      CurrentChoiceIndex     = 0;
      isChoiceIndexSelected  = false;
      IsDialoguePlaying      = false;
      inkStory               = null;
    }

    #region PARTICIPANT MODIFICATION METHODS
    public void AddDialogueParticipant(DialogueParticipant _participant)
    {
      if (_participant != null)
        participants.Add(_participant.Name.ToLower(), _participant);
    }

    public void RemoveDialogueParticipant(string _participantName)
    {
      participants.Remove(_participantName.ToLower());
    }

    public void RemoveDialogueParticipant(DialogueParticipant _participant)
    {
      if (_participant != null)
        RemoveDialogueParticipant(_participant.Name);
    }
    #endregion

    #region DIALOGUE CONTROL
    public void StartDialogue(TextAsset _inkAsset)
    {
      inkStory          = new Story(_inkAsset.text);
      IsDialoguePlaying = true;

      ParseTags(inkStory.globalTags);
      ContinueDialogue();
    }

    public void ContinueDialogue()
    {
      if (!IsDialoguePlaying || inkStory == null)
        return;

      participants[currentParticipantName].SetSpeechBubbleVisibility(false);

      if (inkStory.canContinue)
      {
        ParseTags(inkStory.currentTags);
        participants[currentParticipantName].SetSpeechBubbleVisibility(true, false);
        participants[currentParticipantName].SetSpeechBubblePosition();
        participants[currentParticipantName].SpeechBubbleText = inkStory.Continue();
        return;
      }

      if (inkStory.currentChoices.Count > 0)
      {
        if (!isChoiceIndexSelected)
        {
          ShowDialogueChoice(CurrentChoiceIndex);
          return;
        }

        inkStory.ChooseChoiceIndex(CurrentChoiceIndex);
        inkStory.Continue();

        CurrentChoiceIndex    = 0;
        isChoiceIndexSelected = false;

        ContinueDialogue();
        return;
      }

      EndDialogue();
    }

    public void EndDialogue()
    {
      ResetParameters();
    }

    internal void ShowNextDialogueChoice(CallbackContext _context)
    {
      if (IsDialoguePlaying)
        ShowDialogueChoice(CurrentChoiceIndex + (int)_context.ReadValue<Vector2>().x);
    }

    private void ShowDialogueChoice(int _choiceIndex)
    {
      if (inkStory.currentChoices.Count == 0)
        return;

      CurrentChoiceIndex    = _choiceIndex;
      isChoiceIndexSelected = true;

      participants[currentParticipantName].SetSpeechBubbleVisibility(true, true);
      participants[currentParticipantName].SpeechBubbleText = inkStory.currentChoices[CurrentChoiceIndex].text;
    }
    #endregion

    #region PARSE TAGS
    private void ParseTags(List<string> _tags)
    {
      foreach (var tag in _tags)
      {
        string[] values = tag.ToLower().Split(" ");

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
          currentParticipantName = _tags[1];
          return;

        default:
          return;
      }
    }
    #endregion
  }
}
