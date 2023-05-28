using UnityEngine;

namespace TheLonelyOne
{
  [System.Serializable]
  public class PlayerData
  {
    public bool    IsFirstLoading = true;
    public Vector3 Position;
    public int     Direction;

    public MainCameraData MainCamera = new MainCameraData();
  }
}
