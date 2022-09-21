using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;


namespace TheLonelyOne.Player
{
  /// <summary>
  /// Основа управления взята из проекта platformer-movement [https://github.com/Dawnosaur/platformer-movement]
  /// </summary>
  [RequireComponent(typeof(Rigidbody2D))]
  public class PlayerMovementController : MonoBehaviour
  {
    #region COMPONENTS
    protected Rigidbody2D rigidbody2d;
    #endregion

    #region PARAMETERS
    protected Vector2 inputVetor;

    [SerializeField] protected float walkingSpeed;
    [SerializeField] protected float runnigSpeed;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float decceleration;
    [SerializeField] protected float velocityPower;
    [SerializeField] protected float friction;
    #endregion

    #region PROPERTIES
    public bool  IsWalking { get; protected set; }
    public bool  IsRunnnig { get; protected set; }
    /// <summary>
    /// Return true if player is not walking/running or sliding (aka rigidbody2d.velocity.x = 0).
    /// </summary>
    public bool  IsMoving { get; protected set; }
    public float CurrentVelocity { get => rigidbody2d.velocity.x; }
    public float CurrentSpeed { get => Mathf.Abs(CurrentVelocity); }
    public int   Direction { get; protected set; }
    #endregion

    protected void Awake()
    {
      rigidbody2d = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
      if (IsMoving)
        GameEvents.Instance.PlayerMoving();
    }

    protected void FixedUpdate()
    {
      if (!GameManager.Instance.PlayerController.CanMove)
        return;

      IsMoving = false;

      if (IsWalking)
        Move(1.0f);

      if (!IsWalking && CurrentSpeed > 0.01f)
        Drag(friction);
    }

    internal void PlayerMovementStarted(CallbackContext _context)
    {
      IsWalking  = true;
      inputVetor = _context.ReadValue<Vector2>();
    }

    internal void PlayerMovementCanceled(CallbackContext _context)
    {
      IsWalking = false;
    }

    protected void Move(float _lerpValue)
    {
      float targetSpeed = inputVetor.x * walkingSpeed;
      float deltaSpeed  = targetSpeed - rigidbody2d.velocity.x;
      float accelRate   = (Mathf.Abs(targetSpeed) < float.Epsilon) ? acceleration : decceleration;
      float movement    = Mathf.Pow(Mathf.Abs(deltaSpeed) * accelRate, velocityPower) * Mathf.Sign(deltaSpeed);

      movement = Mathf.Lerp(rigidbody2d.velocity.x, movement, _lerpValue);
      rigidbody2d.AddForce(movement * Vector2.right);

      Direction = (int)Mathf.Sign(rigidbody2d.velocity.x);
      IsMoving  = true;
    }

    protected void Drag(float _amount)
    {
      float force = Mathf.Abs(rigidbody2d.velocity.x) * _amount;
      force *= -Mathf.Sign(rigidbody2d.velocity.x);

      rigidbody2d.AddForce(force * Vector2.right, ForceMode2D.Impulse);
      IsMoving = true;
    }
  }
}
