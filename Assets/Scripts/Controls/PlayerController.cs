using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TheLonelyOne.Player
{
  public class PlayerController : MonoBehaviour, ICharacter
  {
    #region COMPOMEMTS
    private PlayerInputActions       inputActions;
    private PlayerMovementController movementCtrl;
    private Animator                 animator;
    #endregion

    private IInteractable interactableObject;

    private void Awake()
    {
      movementCtrl = GetComponent<PlayerMovementController>();
      animator     = GetComponent<Animator>();

      SetUpPlayerInputAction();
    }

    private void Start()
    {
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
      inputActions.Player.Movement.started  += movementCtrl.PlayerMovementStarted;
      inputActions.Player.Movement.canceled += movementCtrl.PlayerMovementCanceled;

      // Interaction
      inputActions.Player.Interact.performed += InteractionPressed;
    }

    private void SetUpAnimation()
    {
      animator.SetBool("IsWalking", movementCtrl.IsWalking);
      animator.SetFloat("Direction", movementCtrl.Direction);
      animator.SetFloat("Speed", Mathf.Abs(movementCtrl.CurrentVelocity));
    }

    private void InteractionPressed(CallbackContext _context)
    {
      if (interactableObject != null)
        interactableObject.Interact();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
      if (_collision.gameObject.GetComponent<IInteractable>() is IInteractable interactable)
        interactableObject = interactable;
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
      if (interactableObject == _collision.gameObject.GetComponent<IInteractable>())
        interactableObject = null;
    }
  }

}
