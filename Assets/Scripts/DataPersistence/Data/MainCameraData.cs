using UnityEngine;

namespace TheLonelyOne
{
  [System.Serializable]
  public class MainCameraData
  {
    public Vector3 Position;

    public bool    IsPositionFixed;
    public Vector3 FixedPosition;
    public byte    UnfixingCondition;
    public byte    AllowedDirection;
  }
}
