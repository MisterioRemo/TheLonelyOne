using UnityEngine;
using System;
using System.Collections.Generic;
using Ink.Runtime;
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
    private bool                                    isChoosingChoie;
    private Dictionary<string, DialogueParticipant> participants;

    private Action<string>                          updateInkStateCallback;
    #endregion

    #region PROPERTIES
    public bool IsDialoguePlaying { get; private set; }
    private int CurrentChoiceIndex { get => currentChoiceIndex;
                                     set {
                                       currentChoiceIndex = value;
                                       if (inkStory && inkStory.currentChoices.Count > 0)
                                       {
                                         currentChoiceIndex = value < 0
                                                              ? inkStory.currentChoices.Count - 1
                                                              : value % inkStory.currentChoices.Count;
                                       }
                                     }
                                   }
    #endregion

    private void Awake()
    {
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
      isChoosingChoie        = false;
      IsDialoguePlaying      = false;
      inkStory               = null;
      updateInkStateCallback = null;
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
    public void StartDialogue(TextAsset _inkAsset, string _inkState, Action<string> _updateInkStateCallback)
    {
      inkStory               = new Story(_inkAsset.text);
      IsDialoguePlaying      = true;
      updateInkStateCallback = _updateInkStateCallback;

      SetStoryState(inkStory, _inkState);
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
                                 out string functionName,
                                 out string[] functionParams))
        {
          DialogueAction.Invoke(functionName, functionParams);
          ContinueDialogue();
          return;
        }

        DrawSpeechBubble(participants[currentParticipantName], inkStory.currentText);
        return;
      }

      if (inkStory.currentChoices.Count > 0)
      {
        isChoosingChoie = true;

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
      updateInkStateCallback(inkStory.state.ToJson());
      ResetParameters();
    }

    private void SetStoryState(Story _inkStory, string _inkState)
    {
      if (!string.IsNullOrEmpty(_inkState))
        _inkStory.state.LoadJson(_inkState);

      _inkStory.ChoosePathString("EntryPoint");
    }

    private void DrawSpeechBubble(DialogueParticipant _dialogueParticipant, string _text, bool _hasChoice = false)
    {
      _dialogueParticipant.SetSpeechBubbleVisibility(true, _hasChoice);
      _dialogueParticipant.SetSpeechBubblePosition();
      _dialogueParticipant.SpeechBubbleText = _text;
    }

    internal void ShowNextDialogueChoice(CallbackContext _context)
    {
      if (IsDialoguePlaying && isChoosingChoie && inkStory.currentChoices.Count > 0)
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
      isChoosingChoie       = false;

      inkStory.ChooseChoiceIndex(_index);
      inkStory.Continue();
    }
    #endregion
  }
}
