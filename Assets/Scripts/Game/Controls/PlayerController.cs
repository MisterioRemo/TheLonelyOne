using System;
using System.Collections;
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
    public bool              CanMove                  { get => movementCtrl.CanMove;
                                                        set => movementCtrl.CanMove = value;
                                                      }
    public MovementDirection BlockMovementInDirection { get => movementCtrl.BlockedDirection;
                                                        set => movementCtrl.BlockedDirection = value;
                                                      }
    public IInteractable     InteractableObject       { get => interactableObject;
                                                        private set {
                                                          interactableObject = value;
                                                          if (interactableObject != null)
                                                            OnInteractableSet?.Invoke(interactableObject);
                                                        }
                                                      }
    #endregion

    #region EVENTS
    public event Action<Vector3>       OnTeleporting;
    public event Action                OnTeleportingEnd;
    public event Action<IInteractable> OnInteractableSet;
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
        InteractableObject = interactable;
    }

    protected void OnTriggerExit2D(Collider2D _collision)
    {
      if (interactableObject == _collision.GetComponent<IInteractable>())
        InteractableObject = null;
    }
    #endregion

    #region METHODS
    protected IEnumerator TeleportEnd(float _duration)
    {
      yield return new WaitForSeconds(_duration);
      OnTeleportingEnd?.Invoke();
    }
    #endregion

    #region INTERFACE
    public void Teleport(Vector3 _position, float _duration = 0.0f)
    {
      transform.position            = _position;
      movementCtrl.BlockedDirection = MovementDirection.None;
      OnTeleporting?.Invoke(_position);
      StartCoroutine(TeleportEnd(_duration));
    }

    public void DetectNearestInteractableObject()
    {
      RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero);

      foreach(var hit in hits)
      {
        if (!hit.collider.CompareTag("Player")
            && hit.collider.GetComponentInChildren<IInteractable>() is IInteractable interactable)
        {
          InteractableObject = interactable;
          break;
        }
      }
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
