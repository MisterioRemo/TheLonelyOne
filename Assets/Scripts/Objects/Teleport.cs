using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLonelyOne
{
  public class Teleport : InteractableObject
  {
    #region PARAMETERS
    [SerializeField] protected Transform  destination;
    [SerializeField] protected GameObject transitionUI;
    [SerializeField] protected float      transitionStartDuration = 1.0f;
    [SerializeField] protected float      transitionDuration = 0.5f;
    [SerializeField] protected float      transitionEndDuration   = 1.0f;

    protected Animator transition;
    #endregion

    #region INTERACTABLE OBJECT
    public override void Interact()
    {
      base.Interact();
      StartCoroutine(StartTransition());
    }

    public override void Save(ref GameData _gameData)
    {
      // Empty
    }

    public override void Load(GameData _gameData)
    {
      // Empty
    }
    #endregion

    protected override void Awake()
    {
      base.Awake();

      transition = transitionUI.GetComponent<Animator>();
    }

    protected IEnumerator StartTransition()
    {
      GameEvents.Instance.AllowPlayerToMove(false);
      transitionUI.SetActive(true);
      yield return new WaitForSeconds(transitionStartDuration);

      if (destination != null)
      {
        GameEvents.Instance.PlayerTeleporting(destination.position);
        yield return new WaitForSeconds(transitionDuration);
      }

      transition.SetTrigger("End");
      yield return new WaitForSeconds(transitionEndDuration);

      transitionUI.SetActive(false);
      GameEvents.Instance.AllowPlayerToMove(true);
    }
  }
}
