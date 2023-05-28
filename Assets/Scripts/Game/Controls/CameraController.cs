using UnityEngine;

namespace TheLonelyOne
{
  [RequireComponent(typeof(BoxCollider2D))]
  public class CameraController : MonoBehaviour, IDataPersistence
  {
    public enum UnfixingCondition : byte
    {
      DirectionChange,
      StayFixed
    }

    #region PARAMETERS
    [SerializeField] protected Rigidbody2D target;
    [SerializeField] protected float       interpolationSpeed;
    [SerializeField] protected Vector2     offset;

    protected BoxCollider2D     boxCollider;
    protected Vector3           fixedPosition;
    protected UnfixingCondition unfixingCondition;
    protected MovementDirection allowedDirection;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// Only horizontal fix.
    /// </summary>
    public bool          IsPositionFixed { get; set; }
    #endregion

    #region IDataPersistence
    public void Save(ref GameData _gameData)
    {
      _gameData.Player.MainCamera.Position          = transform.position;
      _gameData.Player.MainCamera.IsPositionFixed   = IsPositionFixed;
      _gameData.Player.MainCamera.FixedPosition     = fixedPosition;
      _gameData.Player.MainCamera.UnfixingCondition = (byte)unfixingCondition;
      _gameData.Player.MainCamera.AllowedDirection  = (byte)allowedDirection;
    }

    public void Load(GameData _gameData)
    {
      if (_gameData.Player.IsFirstLoading)
        return;

      transform.position = _gameData.Player.MainCamera.Position;
      IsPositionFixed    = _gameData.Player.MainCamera.IsPositionFixed;
      fixedPosition      = _gameData.Player.MainCamera.FixedPosition;
      unfixingCondition  = (UnfixingCondition)_gameData.Player.MainCamera.UnfixingCondition;
      allowedDirection   = (MovementDirection)_gameData.Player.MainCamera.AllowedDirection;
    }
    #endregion

    #region LIFECYCLE
    protected void Awake()
    {
      boxCollider      = GetComponent<BoxCollider2D>();
      boxCollider.size = CalculateColliderSize(GetComponent<Camera>());

      if (target && target.GetComponent<Player.PlayerController>() is Player.PlayerController playerCtrl)
      {
        playerCtrl.OnTeleporting    += Teleport;
        playerCtrl.OnTeleportingEnd += SwitchEnabled;
      }
    }

    protected void LateUpdate()
    {
      if (IsPositionFixed && IsUnfixedConditionFulfilled())
        IsPositionFixed = false;

      Vector3 newPosition = CalculateCameraPosition(target.position);
      transform.position  = IsPositionFixed
                            ? newPosition
                            : Vector3.Lerp(transform.position,
                                           newPosition,
                                           interpolationSpeed * Time.deltaTime);
    }

    protected void OnDestroy()
    {
      if (target && target.GetComponent<Player.PlayerController>() is Player.PlayerController playerCtrl)
      {
        playerCtrl.OnTeleporting    -= Teleport;
        playerCtrl.OnTeleportingEnd -= SwitchEnabled;
      }
    }
    #endregion

    #region METHODS
    protected Vector3 CalculateCameraPosition(Vector3 _targetPosition)
    {
      if (!IsPositionFixed)
        return new Vector3(_targetPosition.x + offset.x, _targetPosition.y + offset.y, transform.position.z);
      else if (unfixingCondition == UnfixingCondition.StayFixed)
        return new Vector3(fixedPosition.x, fixedPosition.y, transform.position.z);
      else
        return new Vector3(fixedPosition.x, _targetPosition.y + offset.y, transform.position.z);
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
          return Utils.GetMovementDirection((int)(target.position.x - transform.position.x)) == allowedDirection;

        case UnfixingCondition.StayFixed:
          return false;

        default:
          return true;
      }
    }

    protected void Teleport(Vector3 _position)
    {
      SwitchEnabled();
      IsPositionFixed = false;
      InstantCameraMove(_position);
    }

    protected void SwitchEnabled()
    {
      boxCollider.enabled = !boxCollider.enabled;
      enabled             = !enabled;
    }
    #endregion

    #region INTERFACE
    public void InstantCameraMove(Vector3 _position)
    {
      transform.position = CalculateCameraPosition(_position);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_fixedPosition"> position in world space</param>
    /// <param name="_blockedDirection">
    /// <param name="_condition"></param>
    public void FixCameraPosition(Vector3 _fixedPosition, MovementDirection _blockedDirection, UnfixingCondition _condition)
    {
      IsPositionFixed   = true;
      fixedPosition     = _fixedPosition;
      allowedDirection  = Utils.GetOppositeDirection(_blockedDirection);
      unfixingCondition = _condition;
    }
    #endregion
  }
}
