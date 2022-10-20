using UnityEngine;
using System;
using System.Collections.Generic;
using Ink.Runtime;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using Zenject;

namespace TheLonelyOne.Dialogue
{
  public partial class DialogueManager : IInitializable, IDisposable
  {
    #region PARAMETERS
    protected Story                                   inkStory;
    protected string                                  currentParticipantName;
    protected int                                     currentChoiceIndex;
    protected bool                                    isChoiceIndexSelected;
    protected bool                                    isChoosingChoie;
    protected Dictionary<string, DialogueParticipant> participants;

    protected Action<string>                          updateInkStateCallback;

    public    readonly DialogueParser Parser;
    protected          DialogueAction dialogueAction;

    [Inject] protected Player.PlayerController playerCtrl;
    [Inject] protected PlayerInputActions      inputActions;
    #endregion

    #region PROPERTIES
    public bool IsDialoguePlaying { get; protected set; }
    protected int CurrentChoiceIndex { get => currentChoiceIndex;
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

    public DialogueManager()
    {
      participants   = new Dictionary<string, DialogueParticipant>();
      Parser         = new DialogueParser(this);
    }

    #region IInitializable
    public void Initialize()
    {
      // т.к. DialogueAction использует Inject, то приходится создавать объект через DiContainer
      dialogueAction = DiContainerRef.Container.Instantiate<DialogueAction>(new object[] { this });

      inputActions.Player.Movement.performed += ShowNextDialogueChoice;

      ResetParameters();
    }
    #endregion

    #region IDisposable
    public void Dispose()
    {
      inputActions.Player.Movement.performed -= ShowNextDialogueChoice;
    }
    #endregion

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

    #region INPUT ACTIONS CALLBACKS
    protected void ShowNextDialogueChoice(CallbackContext _context)
    {
      if (IsDialoguePlaying && isChoosingChoie && inkStory.currentChoices.Count > 0)
        ShowDialogueChoice(CurrentChoiceIndex + (int)_context.ReadValue<Vector2>().x);
    }
    #endregion

    #region INTREFACE
    public void StartDialogue(TextAsset _inkAsset, string _inkState, Action<string> _updateInkStateCallback)
    {
      inkStory               = new Story(_inkAsset.text);
      IsDialoguePlaying      = true;
      updateInkStateCallback = _updateInkStateCallback;
      playerCtrl.CanMove     = false;

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
          dialogueAction.Invoke(functionName, functionParams);
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
      playerCtrl.CanMove = true;
      ResetParameters();
    }
    #endregion

    #region METHODS
    protected void ResetParameters()
    {
      currentParticipantName = null;
      CurrentChoiceIndex     = 0;
      isChoiceIndexSelected  = false;
      isChoosingChoie        = false;
      IsDialoguePlaying      = false;
      inkStory               = null;
      updateInkStateCallback = null;
    }

    protected void SetStoryState(Story _inkStory, string _inkState)
    {
      if (!string.IsNullOrEmpty(_inkState))
        _inkStory.state.LoadJson(_inkState);

      _inkStory.ChoosePathString("EntryPoint");
    }

    protected void DrawSpeechBubble(DialogueParticipant _dialogueParticipant, string _text, bool _hasChoice = false)
    {
      _dialogueParticipant.SetSpeechBubbleVisibility(true, _hasChoice);
      _dialogueParticipant.SetSpeechBubblePosition();
      _dialogueParticipant.SpeechBubbleText = _text;
    }

    protected void ShowDialogueChoice(int _choiceIndex)
    {
      CurrentChoiceIndex    = _choiceIndex;
      isChoiceIndexSelected = true;

      Parser.ParseTags(Parser.GetCustomTags(inkStory.currentChoices[CurrentChoiceIndex].text));
      DrawSpeechBubble(participants[currentParticipantName], inkStory.currentChoices[CurrentChoiceIndex].text, true);
    }

    protected void ChooseChoiceIndex(int _index)
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
