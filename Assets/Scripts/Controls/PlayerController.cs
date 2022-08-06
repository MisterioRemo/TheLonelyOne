using UnityEngine;

namespace TheLonelyOne.Player
{
  public class PlayerController : MonoBehaviour, ICharacter
  {
    #region COMPOMEMTS
    private PlayerInputActions       inputActions;
    private PlayerMovementController movementCtrl;
    private Animator                 animator;
    #endregion

    private void Awake()
    {
      movementCtrl = GetComponent<PlayerMovementController>();
      animator     = GetComponent<Animator>();

      SetUpPlayerInputAction();

      GameEvents.Instance.OnPlayerMoving += SetUpAnimation;
    }

    private void Update()
    {
      if (movementCtrl.IsMoving)
        GameEvents.Instance.PlayerMoving();
    }

    private void OnDestroy()
    {
      GameEvents.Instance.OnPlayerMoving -= SetUpAnimation;
    }

    private void SetUpPlayerInputAction()
    {
      inputActions = new PlayerInputActions();

      inputActions.Player.Enable();

      // Movement
      inputActions.Player.Movement.started   += movementCtrl.PlayerMovementStarted;
      inputActions.Player.Movement.canceled  += movementCtrl.PlayerMovementCanceled;
    }

    private void SetUpAnimation()
    {
      animator.SetBool("IsWalking", movementCtrl.IsWalking);
      animator.SetFloat("Direction", movementCtrl.Direction);
      animator.SetFloat("Speed", Mathf.Abs(movementCtrl.CurrentVelocity));
    }
  }

}
