using System.Collections.Generic;
using UnityEngine;
using TheLonelyOne.Dialogue;
using TMPro;

namespace TheLonelyOne
{
  public class DialogueParticipant : MonoBehaviour, IInteractable
  {
    #region PARAMETERS
    [Header("Participant")]
    [SerializeField] protected string         participantName;
    [SerializeField] protected SpriteRenderer participantSpriteRenderer;

    [Header("Ink")]
    [SerializeField] protected TextAsset inkAsset;

    [Header("Speech bubble")]
    [SerializeField] protected Vector2 speechBubbleOffset;

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

    #region IInteractable
    public virtual void Interact()
    {
      if (!DialogueManager.Instance.IsDialoguePlaying)
        DialogueManager.Instance.StartDialogue(inkAsset);
      else
        DialogueManager.Instance.ContinueDialogue();
    }
    #endregion

    protected void Awake()
    {
      speechBubbleCanvas = gameObject.transform.Find("DialogueContainer").gameObject;
      speechBubble       = speechBubbleCanvas.transform.Find("DialogueBubble").gameObject;
      speechBubbleText   = speechBubbleCanvas.GetComponentInChildren<TextMeshProUGUI>();
      speechBubbleArrows = new List<GameObject> {
        speechBubbleCanvas.transform.Find("DialogueBubble/BubbleImage/RightArrow").gameObject,
        speechBubbleCanvas.transform.Find("DialogueBubble/BubbleImage/LeftArrow").gameObject
      };
    }

    protected void Start()
    {
      SetSpeechBubbleVisibility(false);
      DialogueManager.Instance.AddDialogueParticipant(this);
    }

    protected void OnDestroy()
    {
      DialogueManager.Instance.RemoveDialogueParticipant(Name);
    }

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
  }
}
