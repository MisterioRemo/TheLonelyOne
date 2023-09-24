using UnityEngine;

namespace TheLonelyOne
{
  public class SpeakableObject : InteractableObject
  {
    #region COMPONENTS
    [SerializeField] protected DialogueParticipant dialogueParticipant;
    #endregion

    #region InteractableObject
    public override void Interact()
    {
      base.Interact();
      dialogueParticipant.Interact();
    }
    #endregion
  }
}
