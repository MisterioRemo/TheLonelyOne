using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TheLonelyOne.Dialogue
{
  public partial class DialogueManager : MonoBehaviour
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
      currentParticipantName = null;
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

      Parser.ParseTags(inkStory.globalTags);
      ContinueDialogue();
    }

    public void ContinueDialogue()
    {
      if (!IsDialoguePlaying || inkStory == null)
        return;

      if (currentParticipantName != null)
        participants[currentParticipantName].SetSpeechBubbleVisibility(false);

      if (inkStory.canContinue)
      {
        inkStory.Continue();
        Parser.ParseTags(inkStory.currentTags);

        if (Parser.ParseFunction(inkStory.currentText,
                                 out GameObject participant,
                                 out string functionName,
                                 out string[] functionParams))
        {
          DialogueAction.Invoke(participant, functionName, functionParams);
          ContinueDialogue();
          return;
        }

        DrawSpeechBubble(participants[currentParticipantName], inkStory.currentText);
        return;
      }

      if (inkStory.currentChoices.Count > 0)
      {
        if (isChoiceIndexSelected)
        {
          ChooseChoiceIndex(CurrentChoiceIndex);
          ContinueDialogue();
          return;
        }

        ShowDialogueChoice(CurrentChoiceIndex);
        return;
      }

      EndDialogue();
    }

    public void EndDialogue()
    {
      ResetParameters();
    }

    private void DrawSpeechBubble(DialogueParticipant _dialogueParticipant, string _text, bool _hasChoice = false)
    {
      _dialogueParticipant.SetSpeechBubbleVisibility(true, _hasChoice);
      _dialogueParticipant.SetSpeechBubblePosition();
      _dialogueParticipant.SpeechBubbleText = _text;
    }

    internal void ShowNextDialogueChoice(CallbackContext _context)
    {
      if (IsDialoguePlaying && inkStory.currentChoices.Count > 0)
        ShowDialogueChoice(CurrentChoiceIndex + (int)_context.ReadValue<Vector2>().x);
    }

    private void ShowDialogueChoice(int _choiceIndex)
    {
      CurrentChoiceIndex    = _choiceIndex;
      isChoiceIndexSelected = true;

      Parser.ParseTags(Parser.GetCustomTags(inkStory.currentChoices[CurrentChoiceIndex].text));
      DrawSpeechBubble(participants[currentParticipantName], inkStory.currentChoices[CurrentChoiceIndex].text, true);
    }

    private void ChooseChoiceIndex(int _index)
    {
      CurrentChoiceIndex    = 0;
      isChoiceIndexSelected = false;

      inkStory.ChooseChoiceIndex(_index);
      inkStory.Continue();
    }
    #endregion
  }
}
