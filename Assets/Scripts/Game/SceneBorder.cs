using UnityEngine;

namespace TheLonelyOne
{
  [RequireComponent(typeof(BoxCollider2D))]
  public class SceneBorder : MonoBehaviour
  {
    public enum BlockedSide : byte
    {
      Both,
      Right,
      Left,
      None
    }

    #region PARAMETERS
    [SerializeField] protected BlockedSide blockedSide;
    [SerializeField] protected bool        doesAffectPlayer;
    [SerializeField] protected bool        doesAffectCamera;

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
        && blockedSide != BlockedSide.None
        && _collision.GetComponent<Player.PlayerController>() is Player.PlayerController _playerController)
      {
        // stop player movement
      }

      if (doesAffectCamera // && _collision.CompareTag("MainCamera")
          && blockedSide != BlockedSide.None
          && _collision.GetComponent<CameraController>() is CameraController _cameraController)
      {
        _cameraController.FixCameraPosition(new Vector3(CalculateExtremePosition(_cameraController.Collider, boxCollider, blockedSide),
                                                        _cameraController.transform.position.y,
                                                        _cameraController.transform.position.z),
                                            CameraController.UnfixingCondition.DirectionChange);
      }
    }

    /// <summary>
    /// Caclulate x coordinate in world space.
    /// </summary>
    /// <param name="_targetCollider"></param>
    /// <param name="_borderCollider"></param>
    /// <param name="_blockedSide"></param>
    /// <returns></returns>
    protected float CalculateExtremePosition(BoxCollider2D _targetCollider, BoxCollider2D _borderCollider, BlockedSide _blockedSide)
    {
      bool targetOnLeftSide = _targetCollider.Distance(_borderCollider).normal.x > 0;
      int  sign             = 0;

      if (targetOnLeftSide && (_blockedSide == BlockedSide.Left || _blockedSide == BlockedSide.Both))
        sign = -1;
      else if (!targetOnLeftSide && (_blockedSide == BlockedSide.Right || _blockedSide == BlockedSide.Both))
        sign = 1;
      
      return transform.TransformPoint(_borderCollider.bounds.center).x
             + sign * (_borderCollider.size.x + _targetCollider.size.x) / 2.0f;
    }
  }
}
