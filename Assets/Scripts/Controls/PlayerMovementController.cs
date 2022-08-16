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
    private Rigidbody2D rigidbody2d;
    #endregion

    #region PARAMETERS
    private Vector2 inputVetor;

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runnigSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velocityPower;
    [SerializeField] private float friction;
    #endregion

    #region PROPERTIES
    public bool  IsWalking { get; private set; }
    public bool  IsRunnnig { get; private set; }
    /// <summary>
    /// Return true if player is not walking/running or sliding (aka rigidbody2d.velocity.x = 0).
    /// </summary>
    public bool  IsMoving { get; private set; }
    public float CurrentVelocity { get => rigidbody2d.velocity.x; }
    public int   Direction { get; private set; }
    #endregion

    private void Awake()
    {
      rigidbody2d  = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      if (Dialogue.DialogueManager.Instance.IsDialoguePlaying)
        return;

      IsMoving = false;

      if (IsWalking)
        Move(1.0f);

      if (!IsWalking && Mathf.Abs(rigidbody2d.velocity.x) > 0.01f)
      {
        Drag(friction);
        IsMoving = true;
      }
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

    private void Move(float _lerpValue)
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

    private void Drag(float _amount)
    {
      float force = Mathf.Abs(rigidbody2d.velocity.x) * _amount;
      force *= -Mathf.Sign(rigidbody2d.velocity.x);

      rigidbody2d.AddForce(force * Vector2.right, ForceMode2D.Impulse);
    }
  }
}
