using UnityEngine;

namespace TheLonelyOne
{
  [RequireComponent(typeof(BoxCollider2D))]
  public class SceneBorder : MonoBehaviour
  {
    #region PARAMETERS
    [SerializeField] protected MovementDirection blockedSide;
    [SerializeField] protected bool              doesAffectPlayer;
    [SerializeField] protected bool              doesAffectCamera;
    [SerializeField] protected bool              setCameraPositionManually;
    [SerializeField] protected Vector3           cameraPosition;

    BoxCollider2D boxCollider;
    #endregion

    #region PROPERTIES
    public bool DoesAffectPlayer { get => doesAffectPlayer;
                                   set => doesAffectPlayer = value;
    }
    public bool DoesAffectCamera { get => doesAffectCamera;
                                   set => doesAffectCamera = value;
                                 }
    #endregion

    protected void Awake()
    {
      boxCollider           = GetComponent<BoxCollider2D>();
      boxCollider.isTrigger = true;
    }

    protected void OnTriggerEnter2D(Collider2D _collision)
    {
      if (DoesAffectPlayer // && _collision.gameObject.tag == "Player"
        && blockedSide != MovementDirection.None
        && _collision.GetComponent<Player.PlayerController>() is Player.PlayerController _playerController)
      {
        _playerController.BlockMovementInDirection = GetTargetBlockedDirection(_collision, boxCollider, blockedSide);
      }

      if (doesAffectCamera // && _collision.CompareTag("MainCamera")
          && blockedSide != MovementDirection.None
          && _collision.GetComponent<CameraController>() is CameraController _cameraController)
      {
        if (setCameraPositionManually)
          _cameraController.FixCameraPosition(cameraPosition,
                                              GetTargetBlockedDirection(_collision, boxCollider, blockedSide),
                                              CameraController.UnfixingCondition.StayFixed);
        else
          _cameraController.FixCameraPosition(new Vector3(CalculateExtremeXPosition((BoxCollider2D)_collision, boxCollider, blockedSide),
                                                          _cameraController.transform.position.y,
                                                          _cameraController.transform.position.z),
                                              GetTargetBlockedDirection(_collision, boxCollider, blockedSide),
                                              CameraController.UnfixingCondition.DirectionChange);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_targetCollider"></param>
    /// <param name="_borderCollider"></param>
    /// <param name="_blockedSide"></param>
    /// <returns>Returns 1 if right movement should be blocked, -1 if left, 0 otherwise.</returns>
    protected int GetBlockedMovementSign(Collider2D _targetCollider, Collider2D _borderCollider, MovementDirection _blockedSide)
    {
      bool targetOnLeftSide = _targetCollider.Distance(_borderCollider).normal.x > 0;

      if (targetOnLeftSide && (_blockedSide == MovementDirection.Left || _blockedSide == MovementDirection.Horizontal))
        return 1;
      else if (!targetOnLeftSide && (_blockedSide == MovementDirection.Right || _blockedSide == MovementDirection.Horizontal))
        return -1;
      else
        return 0;
    }

    protected MovementDirection GetTargetBlockedDirection(Collider2D _targetCollider, Collider2D _borderCollider, MovementDirection _blockedSide)
    {
      return Utils.GetMovementDirection(GetBlockedMovementSign(_targetCollider, _borderCollider, _blockedSide));
    }

    /// <summary>
    /// Caclulate x coordinate in world space.
    /// </summary>
    /// <param name="_targetCollider"></param>
    /// <param name="_borderCollider"></param>
    /// <param name="_blockedSide"></param>
    /// <returns></returns>
    protected float CalculateExtremeXPosition(BoxCollider2D _targetCollider, BoxCollider2D _borderCollider, MovementDirection _blockedSide)
    {
      int sign = GetBlockedMovementSign(_targetCollider, _borderCollider, _blockedSide);

      return transform.TransformPoint(_borderCollider.bounds.center).x
             - sign * (_borderCollider.size.x + _targetCollider.size.x) / 2.0f;
    }
  }
}
