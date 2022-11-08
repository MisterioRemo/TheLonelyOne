using System.Collections.Generic;
using UnityEngine;
using TheLonelyOne.Dialogue;
using TMPro;
using Zenject;

namespace TheLonelyOne
{
  public class DialogueParticipant : MonoBehaviour, IInteractable
  {
    #region PARAMETERS
    [SerializeField] protected string id;

    [Header("Participant")]
    [SerializeField] protected string         participantName;
    [SerializeField] protected SpriteRenderer participantSpriteRenderer;

    [Header("Ink")]
    [SerializeField] protected TextAsset inkAsset;

    protected string inkState;

    [Header("Speech bubble")]
    [SerializeField] protected Vector2 speechBubbleOffset;

    [Inject] protected DialogueManager dialogueManager;

    protected GameObject       speechBubbleCanvas;
    protected GameObject       speechBubble;
    protected TextMeshProUGUI  speechBubbleText;
    protected List<GameObject> speechBubbleArrows;
    #endregion

    #region PROPERTIES
    public string Name { get => participantName; }
    public string SpeechBubbleText { get => speechBubbleText.text;
                                     set => speechBubbleText.text = value;
                                   }
    #endregion

    [ContextMenu("Generate guid fo id")]
    protected void GenerateGuid() => id = Utils.GenerateGuid();

    #region IInteractable
    public virtual void PreInteract()
    {
      // Empty
    }

    public virtual void Interact()
    {
      if (!dialogueManager.IsDialoguePlaying)
        dialogueManager.StartDialogue(inkAsset, inkState, UpdateInkState);
      else
        dialogueManager.ContinueDialogue();
    }

    public virtual void PostInteract()
    {
      // Empty
    }
    #endregion

    #region IDataPersistence
    public virtual void Save(ref GameData _gameData)
    {
      if (!string.IsNullOrEmpty(inkState))
        _gameData.DialogueAssetStates[id] = inkState;
    }

    public virtual void Load(GameData _gameData)
    {
      _gameData.DialogueAssetStates.TryGetValue(id, out inkState);
    }
    #endregion

    #region LIFECYCLE
    protected virtual void Awake()
    {
      speechBubbleCanvas = gameObject.transform.Find("DialogueContainer").gameObject;
      speechBubble       = speechBubbleCanvas.transform.Find("DialogueBubble").gameObject;
      speechBubbleText   = speechBubbleCanvas.GetComponentInChildren<TextMeshProUGUI>();
      speechBubbleArrows = new List<GameObject> {
        speechBubbleCanvas.transform.Find("DialogueBubble/BubbleImage/RightArrow").gameObject,
        speechBubbleCanvas.transform.Find("DialogueBubble/BubbleImage/LeftArrow").gameObject
      };
    }

    protected virtual void Start()
    {
      SetSpeechBubbleVisibility(false);
      dialogueManager.AddDialogueParticipant(this);
    }

    protected virtual void OnDestroy()
    {
      dialogueManager.RemoveDialogueParticipant(Name);
    }
    #endregion

    #region INTERFACE
    public void SetSpeechBubbleVisibility(bool _isBubbleVisible, bool _isArrowVisible = false)
    {
      SetSpeechBubbleArrowVisibility(_isArrowVisible);
      speechBubbleCanvas.SetActive(_isBubbleVisible);
    }

    public void SetSpeechBubbleArrowVisibility(bool _isVisible)
    {
      foreach (var arrow in speechBubbleArrows)
        arrow.SetActive(_isVisible);
    }

    public void SetSpeechBubblePosition()
    {
      Vector3 participantPosition  = participantSpriteRenderer.gameObject.transform.position;
      Vector3 spriteExtents        = participantSpriteRenderer.sprite.bounds.extents;
      Vector3 speechBubblePosition = new Vector3(participantPosition.x + speechBubbleOffset.x + spriteExtents.x,
                                                 participantPosition.y + speechBubbleOffset.y + spriteExtents.y,
                                                 participantPosition.z);

      speechBubble.transform.position = Camera.main.WorldToScreenPoint(speechBubblePosition);
    }

    public void UpdateInkState(string _savedJson)
    {
      inkState = _savedJson;
    }
    #endregion
  }
}
