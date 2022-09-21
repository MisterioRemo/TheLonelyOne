using UnityEngine;

namespace TheLonelyOne
{
  [RequireComponent(typeof(BoxCollider2D))]
  public class CameraController : MonoBehaviour
  {
    public enum UnfixingCondition : byte
    {
      DirectionChange,
      StayFixed
    }

    #region COMPONENTS
    [SerializeField] protected Rigidbody2D target;

    protected BoxCollider2D boxCollider;
    #endregion

    #region PARAMETERS
    [SerializeField] protected float interpolationSpeed;
    [SerializeField] protected Vector2 offset;

    protected float             horizontalDirection;
    protected Vector3           fixedPosition;
    protected UnfixingCondition unfixingCondition;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// Only horizontal fix.
    /// </summary>
    public bool          IsPositionFixed { get; set; }
    public BoxCollider2D Collider { get => boxCollider; }
    #endregion

    #region LIFECYCLE
    protected void Awake()
    {
      boxCollider      = GetComponent<BoxCollider2D>();
      boxCollider.size = CalculateColliderSize(GetComponent<Camera>());
    }

    protected void Start()
    {
      GameEvents.Instance.OnPlayerTeleporting += OnPlayerTeleport;
    }

    protected void LateUpdate()
    {
      if (IsPositionFixed && IsUnfixedConditionFulfilled())
        IsPositionFixed = false;

      horizontalDirection = target.position.x - transform.position.x;
      Vector3 newPosition = CalculateCameraPosition(target.position);
      transform.position  = IsPositionFixed
                            ? new Vector3(fixedPosition.x, newPosition.y, newPosition.y)
                            : Vector3.Lerp(transform.position,
                                           newPosition,
                                           interpolationSpeed * Time.deltaTime);
    }

    protected void OnDestroy()
    {
      GameEvents.Instance.OnPlayerTeleporting -= OnPlayerTeleport;
    }
    #endregion

    protected Vector3 CalculateCameraPosition(Vector3 _targetPosition)
    {
      return new Vector3(_targetPosition.x + offset.x,
                         _targetPosition.y + offset.y,
                         transform.position.z);
    }

    protected Vector2 CalculateColliderSize(Camera _camera)
    {
      float ratio  = Screen.width/(float)Screen.height;
      float height = _camera.orthographicSize * 2;
      float width  = height * ratio;

      return new Vector2(width, height);
    }

    protected bool IsUnfixedConditionFulfilled()
    {
      switch (unfixingCondition)
      {
        case UnfixingCondition.DirectionChange:
          return horizontalDirection * (target.position.x - transform.position.x) < 0;

        case UnfixingCondition.StayFixed:
        default:
          return true;
      }
    }

    protected void OnPlayerTeleport(Vector3 _position)
    {
      InstantCameraMove(_position);
      IsPositionFixed = false;
    }

    #region INTERFACE
    public void InstantCameraMove(Vector3 _position)
    {
      transform.position = CalculateCameraPosition(_position);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_fixedPosition"> position in world space</param>
    /// <param name="_condition"></param>
    public void FixCameraPosition(Vector3 _fixedPosition, UnfixingCondition _condition)
    {
      IsPositionFixed   = true;
      fixedPosition     = _fixedPosition;
      unfixingCondition = _condition;
    }
    #endregion
  }
}
