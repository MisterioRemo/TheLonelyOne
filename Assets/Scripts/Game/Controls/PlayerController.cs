using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TheLonelyOne.Player
{
  public class PlayerController : MonoBehaviour, ICharacter, IDataPersistence
  {
    #region PARAMETERS
    protected Animator                 animator;
    private   IInteractable[]          interactableObject;
    private   HashSet<IInteractable[]> interactableObjectsInArea;

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
    public IInteractable[]   InteractableObject       { get => interactableObject;
                                                        private set {
                                                          interactableObject = value;
                                                          if (interactableObject != null)
                                                            OnInteractableSet?.Invoke(interactableObject);
                                                        }
                                                      }
    #endregion

    #region EVENTS
    public event Action<Vector3>         OnTeleporting;
    public event Action                  OnTeleportingEnd;
    public event Action<IInteractable[]> OnInteractableSet;
    #endregion

    #region IDataPersistence
    public void Save(ref GameData _gameData)
    {
      _gameData.Player.IsFirstLoading = false;
    }

    public void Load(GameData _gameData)
    {
    }
    #endregion

    #region LIFECYCLE
    protected virtual void Awake()
    {
      animator                  = GetComponent<Animator>();
      interactableObjectsInArea = new HashSet<IInteractable[]>(new ObjectArrayComparer<IInteractable>());
    }

    protected virtual void Start()
    {
      inputActions.Player.Interact.performed += InteractionPressed;

      movementCtrl.OnMovingStateChange += SetWalkingAnimation;
      movementCtrl.OnDirectionChange   += SetDirectionAnimation;
      movementCtrl.OnSpeedChange       += SetSpeedAnimation;

      DetectNearestInteractableObject();
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
      if (interactableObject != null && interactableObject.Length != 0)
      {
        foreach (var obj in interactableObject)
          obj.Interact();
      }
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
      if (TryGetInteractableComponents(_collision.gameObject, out IInteractable[] interactable))
      {
        InteractableObject = interactable;
        interactableObjectsInArea.Add(interactable);
      }
    }

    protected void OnTriggerExit2D(Collider2D _collision)
    {
      if (interactableObject != null
          && interactableObject.Contains(_collision.GetComponent<IInteractable>()))
      {
        interactableObjectsInArea.Remove(InteractableObject);
        InteractableObject = null;
        if (interactableObjectsInArea.Count != 0)
          InteractableObject = interactableObjectsInArea.First();
      }
    }
    #endregion

    #region METHODS
    protected IEnumerator TeleportEnd(float _duration)
    {
      yield return new WaitForSeconds(_duration);
      OnTeleportingEnd?.Invoke();
    }

    private bool TryGetInteractableComponents(GameObject _object, out IInteractable[] _interactableObject)
    {
      _interactableObject = _object.GetComponents<IInteractable>();

      if (_interactableObject != null && _interactableObject.Length > 0)
        return true;

      _interactableObject = null;
      return false;
    }
    #endregion

    #region INTERFACE
    public void Teleport(Vector3 _position, float _duration = 0.0f)
    {
      transform.position            = _position;
      movementCtrl.BlockedDirection = MovementDirection.None;

      interactableObjectsInArea.Clear();
      OnTeleporting?.Invoke(_position);
      StartCoroutine(TeleportEnd(_duration));
    }

    public void DetectNearestInteractableObject()
    {
      BoxCollider2D playerCollider   = GetComponent<BoxCollider2D>();
      Collider2D[]  overlapColliders = Physics2D.OverlapAreaAll(playerCollider.bounds.min,
                                                                playerCollider.bounds.max,
                                                                LayerMask.GetMask("Interactable"));
      foreach (var collider in overlapColliders)
      {
        if (!collider.CompareTag("Player")
            && TryGetInteractableComponents(collider.gameObject, out IInteractable[] interactable))
        {
          InteractableObject = interactable;
          interactableObjectsInArea.Add(interactable);
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
