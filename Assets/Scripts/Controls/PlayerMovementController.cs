using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheLonelyOne.Player
{
  public class PlayerMovementController : MonoBehaviour
  {
    [SerializeField]
    private float walkingSpeed;
    public  float WalkingSpeed { get => walkingSpeed; private set => walkingSpeed = value; }

    [SerializeField]
    private float runnigSpeed;
    public float RunningSpeed { get => runnigSpeed; private set => runnigSpeed = value; }
  }
}
