using UnityEngine;

namespace TheLonelyOne
{
  [CreateAssetMenu(fileName = "MovementParametersData", menuName = "TheLonelyOne/MovementParameters")]
  public class MovementParameters : ScriptableObject
  {
    public float walkingSpeed;
    public float runnigSpeed;
    public float acceleration;
    public float decceleration;
    public float velocityPower;
    public float friction;
  }
}
