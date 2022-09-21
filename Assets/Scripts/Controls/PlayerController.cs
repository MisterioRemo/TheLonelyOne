using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TheLonelyOne.Player
{
  public class PlayerController : MonoBehaviour, ICharacter
  {
    #region COMPONENTS
    protected PlayerInputActions       inputActions;
    protected PlayerMovementController movementCtrl;
    protected Animator                 animator;
    #endregion

    #region PARAMETERS
    protected IInteractable interactableObject;
    #endregion

    #region PROPERTIES
    public bool CanMove { get; set; } = true;
    #endregion

    #region LIFECYCLE
    protected void Awake()
    {
      movementCtrl = GetComponent<PlayerMovementController>();
      animator     = GetComponent<Animator>();
    }

    protected void Start()
    {
      SetUpPlayerInputAction();
      GameEvents.Instance.OnPlayerMoving      += SetUpAnimation;
      GameEvents.Instance.OnPlayerTeleporting += Teleport;
      GameEvents.Instance.OnAllowPlayerToMove += AllowToMove;
    }

    protected void OnDestroy()
    {
      // Movement
      inputActions.Player.Movement.started  -= movementCtrl.PlayerMovementStarted;
      inputActions.Player.Movement.canceled -= movementCtrl.PlayerMovementCanceled;

      // Interaction
      inputActions.Player.Interact.performed -= InteractionPressed;
      inputActions.Player.Movement.performed -= Dialogue.DialogueManager.Instance.ShowNextDialogueChoice;

      // Animation
      GameEvents.Instance.OnPlayerMoving      -= SetUpAnimation;

      GameEvents.Instance.OnPlayerTeleporting -= Teleport;
      GameEvents.Instance.OnAllowPlayerToMove -= AllowToMove;
    }
    #endregion

    protected void SetUpPlayerInputAction()
    {
      inputActions = new PlayerInputActions();

      inputActions.Player.Enable();

      // Movement
      inputActions.Player.Movement.started  += movementCtrl.PlayerMovementStarted;
      inputActions.Player.Movement.canceled += movementCtrl.PlayerMovementCanceled;

      // Interaction
      inputActions.Player.Interact.performed += InteractionPressed;
      inputActions.Player.Movement.performed += Dialogue.DialogueManager.Instance.ShowNextDialogueChoice;
    }

    protected void SetUpAnimation()
    {
      animator.SetBool("IsWalking", movementCtrl.IsWalking);
      animator.SetFloat("Direction", movementCtrl.Direction);
      animator.SetFloat("Speed", Mathf.Abs(movementCtrl.CurrentVelocity));
    }

    protected void InteractionPressed(CallbackContext _context)
    {
      if (interactableObject != null)
        interactableObject.Interact();
    }

    protected void OnTriggerEnter2D(Collider2D _collision)
    {
      if (_collision.GetComponentInChildren<IInteractable>() is IInteractable interactable)
        interactableObject = interactable;
    }

    protected void OnTriggerExit2D(Collider2D _collision)
    {
      if (interactableObject == _collision.GetComponent<IInteractable>())
        interactableObject = null;
    }

    protected void AllowToMove(bool _canMove)
    {
      CanMove = _canMove;
    }

    #region INTERFACE
    public void Teleport(Vector3 _position)
    {
      transform.position = _position;
    }

    public void ChangeInputActionsMap(InputActionsMap _map)
    {
      ChangeInputActionsMap(_map.ToString());
    }

    public void ChangeInputActionsMap(string _mapName)
    {
      switch (_mapName)
      {
        case "Player":
          inputActions.UI.Disable();
          inputActions.Player.Enable();
          return;

        case "UI":
          inputActions.Player.Disable();
          inputActions.UI.Enable();
          return;

        default:
          return;
      }
    }
    #endregion
  }

}
