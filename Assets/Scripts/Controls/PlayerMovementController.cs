using System;
using UnityEngine;
using Zenject;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;


namespace TheLonelyOne.Player
{
  /// <summary>
  /// Основа управления взята из проекта platformer-movement [https://github.com/Dawnosaur/platformer-movement]
  /// </summary>
  [RequireComponent(typeof(Rigidbody2D))]
  public class PlayerMovementController : MonoBehaviour
  {
    #region PARAMETERS
    protected Rigidbody2D rigidbody2d;
    protected Vector2     inputVetor;

    protected bool wasMoving;
    protected bool isMoving;
    protected int  direction;

    [Inject] protected PlayerInputActions inputActions;
    #endregion

    #region PROPERTIES
    public MovementParameters Parameters { get; set; }
    public bool               CanMove { get; set; } = true;
    public bool               IsWalking { get; protected set; }
    public bool               IsRunnnig { get; protected set; }
    public float CurrentVelocity { get => rigidbody2d.velocity.x; }
    public float CurrentSpeed { get => Mathf.Abs(CurrentVelocity); }
    public int   Direction { get => direction;
                             protected set
                             {
                               if (direction != value)
                               {
                                 direction = value;
                                 OnDirectionChange?.Invoke(direction);
                               }
                             }
                           }
    #endregion

    #region EVENTS
    public event Action<float> OnSpeedChange;
    public event Action<int>   OnDirectionChange;
    public event Action<bool>  OnMovingStateChange;
    #endregion

    #region LIFECYCLE
    protected virtual void Awake()
    {
      rigidbody2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
      inputActions.Player.Movement.started  += PlayerMovementStarted;
      inputActions.Player.Movement.canceled += PlayerMovementCanceled;
    }

    protected void Update()
    {
      if (wasMoving != isMoving)
      {
        OnMovingStateChange?.Invoke(isMoving);
        wasMoving = isMoving;
      }

      if (isMoving)
        OnSpeedChange?.Invoke(CurrentSpeed);
    }

    protected virtual void FixedUpdate()
    {
      if (!CanMove)
        return;

      isMoving = false;

      if (IsWalking)
        Move(1.0f);

      if (!IsWalking && CurrentSpeed > 0.01f)
        Drag(Parameters.friction);
    }

    protected virtual void OnDestroy()
    {
      inputActions.Player.Movement.started  -= PlayerMovementStarted;
      inputActions.Player.Movement.canceled -= PlayerMovementCanceled;
    }
    #endregion

    #region INPUT ACTIONS CALLBACKS
    protected virtual void PlayerMovementStarted(CallbackContext _context)
    {
      IsWalking  = true;
      inputVetor = _context.ReadValue<Vector2>();
    }

    protected virtual void PlayerMovementCanceled(CallbackContext _context)
    {
      IsWalking = false;
    }
    #endregion

    #region METHODS
    protected virtual void Move(float _lerpValue)
    {
      float targetSpeed = inputVetor.x * Parameters.walkingSpeed;
      float deltaSpeed  = targetSpeed - rigidbody2d.velocity.x;
      float accelRate   = (Mathf.Abs(targetSpeed) < float.Epsilon) ? Parameters.acceleration : Parameters.decceleration;
      float movement    = Mathf.Pow(Mathf.Abs(deltaSpeed) * accelRate, Parameters.velocityPower) * Mathf.Sign(deltaSpeed);

      movement = Mathf.Lerp(rigidbody2d.velocity.x, movement, _lerpValue);
      rigidbody2d.AddForce(movement * Vector2.right);

      Direction = (int)Mathf.Sign(rigidbody2d.velocity.x);
      isMoving  = true;
    }

    protected virtual void Drag(float _amount)
    {
      float force = Mathf.Abs(rigidbody2d.velocity.x) * _amount;
      force *= -Mathf.Sign(rigidbody2d.velocity.x);

      rigidbody2d.AddForce(force * Vector2.right, ForceMode2D.Impulse);
      isMoving = true;
    }
    #endregion
  }
}
