using System;
using UnityEngine;
using Zenject;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TheLonelyOne.Player
{
  public class PlayerController : MonoBehaviour, ICharacter
  {
    #region PARAMETERS
    protected Animator      animator;
    protected IInteractable interactableObject;

    [Inject] protected PlayerInputActions       inputActions;
    [Inject] protected PlayerMovementController movementCtrl;
    #endregion

    #region PROPERTIES
    public bool CanMove { get; set; } = true;
    #endregion

    #region EVENTS
    public event Action<Vector3> OnTeleporting;
    #endregion

    #region LIFECYCLE
    protected virtual void Awake()
    {
      animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
      inputActions.Player.Interact.performed += InteractionPressed;

      movementCtrl.OnMovingStateChange += SetWalkingAnimation;
      movementCtrl.OnDirectionChange   += SetDirectionAnimation;
      movementCtrl.OnSpeedChange       += SetSpeedAnimation;
    }

    protected virtual void OnDestroy()
    {
      inputActions.Player.Interact.performed -= InteractionPressed;

      movementCtrl.OnMovingStateChange -= SetWalkingAnimation;
      movementCtrl.OnDirectionChange   -= SetDirectionAnimation;
      movementCtrl.OnSpeedChange       -= SetSpeedAnimation;
    }
    #endregion

    #region INPUT ACTIONS CALLBACKS
    protected void InteractionPressed(CallbackContext _context)
    {
      if (interactableObject != null)
        interactableObject.Interact();
    }
    #endregion

    #region ANIMATION CALLBACKS
    protected void SetWalkingAnimation(bool _isWalking)
    {
      animator.SetBool("IsWalking", _isWalking);
    }

    protected void SetDirectionAnimation(int _direction)
    {
      animator.SetFloat("Direction", _direction);
    }

    protected void SetSpeedAnimation(float _speed)
    {
      animator.SetFloat("Speed", _speed);
    }
    #endregion

    #region COLLISIONS
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
    #endregion

    #region INTERFACE
    public void Teleport(Vector3 _position)
    {
      transform.position = _position;
      OnTeleporting?.Invoke(_position);
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
