using UnityEngine;
using UnityEngine.InputSystem;


namespace TheLonelyOne.Player
{
  /// <summary>
  /// Основа управления взята из проекта platformer-movement [https://github.com/Dawnosaur/platformer-movement]
  /// </summary>
  [RequireComponent(typeof(Rigidbody2D))]
  public class PlayerMovementController : MonoBehaviour
  {
    #region COMPOMEMTS
    private Rigidbody2D        rigidbody2d;
    private PlayerInputActions inputActions;
    #endregion

    #region STATE PARAMETERS
    public bool IsWalking { get; private set; }
    public bool IsRunnnig { get; private set; }
    public float CurrentVelocity { get => rigidbody2d.velocity.x; }
    public int Direction { get; private set; }

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runnigSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velocityPower;
    [SerializeField] private float friction;
    #endregion

    private void Awake()
    {
      rigidbody2d  = GetComponent<Rigidbody2D>();
      inputActions = new PlayerInputActions();

      inputActions.Player.Enable();
      inputActions.Player.Movement.started  += (_context) => IsWalking = true;
      inputActions.Player.Movement.canceled += (_context) => IsWalking = false;
      }

    private void FixedUpdate()
    {
      if (IsWalking)
        Move(1.0f);

      if (!IsWalking && Mathf.Abs(rigidbody2d.velocity.x) > 0.01f)
        Drag(friction);
    }

    private void Move(float _lerpValue)
    {
      Vector2 inputVetor  = inputActions.Player.Movement.ReadValue<Vector2>();
      float   targetSpeed = inputVetor.x * walkingSpeed;
      float   deltaSpeed  = targetSpeed - rigidbody2d.velocity.x;
      float   accelRate   = (Mathf.Abs(targetSpeed) < float.Epsilon) ? acceleration : decceleration;
      float   movement    = Mathf.Pow(Mathf.Abs(deltaSpeed) * accelRate, velocityPower) * Mathf.Sign(deltaSpeed);

      movement = Mathf.Lerp(rigidbody2d.velocity.x, movement, _lerpValue);
      rigidbody2d.AddForce(movement * Vector2.right);

      Direction = (int)Mathf.Sign(rigidbody2d.velocity.x);
    }

    private void Drag(float _amount)
    {
      float force = Mathf.Abs(rigidbody2d.velocity.x) * _amount;
      force *= -Mathf.Sign(rigidbody2d.velocity.x);

      rigidbody2d.AddForce(force * Vector2.right, ForceMode2D.Impulse);
    }
  }
}
