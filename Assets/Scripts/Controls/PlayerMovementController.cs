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

    [SerializeField]
    private float walkingSpeed;
    public  float WalkingSpeed
    {
      get => walkingSpeed;
      private set => walkingSpeed = value;
    }

    [SerializeField]
    private float runnigSpeed;
    public  float RunningSpeed
    {
      get => runnigSpeed;
      private set => runnigSpeed = value;
    }

    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velocityPower;
    [SerializeField] private float friction;
    #endregion

    private float walkingTime;

    private void Awake()
    {
      rigidbody2d  = GetComponent<Rigidbody2D>();
      inputActions = new PlayerInputActions();

      inputActions.Player.Enable();
      inputActions.Player.Movement.started += StartMovement;
    }

    private void FixedUpdate()
    {
      if (IsWalking)
        Move(1.0f);
    }

    private void StartMovement(InputAction.CallbackContext _context)
    {
      IsWalking   = true;
      walkingTime = 0.0f;
    }

    private void Move(float _lerpValue)
    {
      if (walkingTime > float.Epsilon && Mathf.Abs(rigidbody2d.velocity.x) < float.Epsilon)
        IsWalking = false;

      Vector2 inputVetor  = inputActions.Player.Movement.ReadValue<Vector2>();
      float   targetSpeed = inputVetor.x * WalkingSpeed;
      float   deltaSpeed  = targetSpeed - rigidbody2d.velocity.x;
      float   accelRate   = (Mathf.Abs(targetSpeed) < float.Epsilon) ? acceleration : decceleration;
      float   movement    = Mathf.Pow(Mathf.Abs(deltaSpeed) * accelRate, velocityPower) * Mathf.Sign(deltaSpeed);

      movement = Mathf.Lerp(rigidbody2d.velocity.x, movement, _lerpValue);
      rigidbody2d.AddForce(movement * Vector2.right);

      walkingTime += Time.fixedDeltaTime;
    }
  }
}
