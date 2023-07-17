using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class MainCharacterHouseCallbackManager : MonoBehaviour
  {
    #region PARAMETERS
    [Inject] protected PlotManager plotManager;

    [Header("Strongbox")]
    [SerializeField] protected UI.CombinationLock    combinationLock;
    [SerializeField] protected int                   strongboxSpriteId;
    [SerializeField] protected UnityEngine.UI.Button strongboxCloseButton;
    [SerializeField] protected DialogueParticipant   strongboxDialogueParticipant;

    protected bool isStrongboxReactionSeen;
    // [Space(10)]
    #endregion

    #region LIFECYCLE
    protected virtual void Start()
    {
      isStrongboxReactionSeen = (bool)strongboxDialogueParticipant.GetInkVariableState("is_reaction_seen");

      if (combinationLock.gameObject.activeSelf)
        combinationLock.OnPuzzleCompleted += OpenStrongbox;
      if (!isStrongboxReactionSeen)
        strongboxCloseButton.onClick.AddListener(StartStrongboxDialogue);
    }

    protected virtual void OnDestroy()
    {
      combinationLock.OnPuzzleCompleted -= OpenStrongbox;
      strongboxCloseButton.onClick.RemoveListener(StartStrongboxDialogue);
    }
    #endregion

    #region METHODS
    private void OpenStrongbox(string _id)
    {
      combinationLock.transform.parent.Find("Background").GetComponent<UISpriteState>().SetSprite(strongboxSpriteId);
      combinationLock.gameObject.SetActive(false);
      plotManager.AddAchievedPlotPoint("StrongboxIsOpen");
    }

    private void StartStrongboxDialogue()
    {
      if (!combinationLock.gameObject.activeSelf && !isStrongboxReactionSeen)
      {
        strongboxDialogueParticipant.StartDialogue("FirstReaction");
        isStrongboxReactionSeen = true;
      }
    }
    #endregion
  }
}
