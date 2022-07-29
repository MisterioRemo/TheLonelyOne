using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLonelyOne.Player
{
  public class Player : MonoBehaviour, ICharacter
  {
    #region COMPOMEMTS
    private PlayerMovementController movementCtrl;
    private Animator                 animator;
    #endregion

    private void Awake()
    {
      movementCtrl = GetComponent<PlayerMovementController>();
      animator     = GetComponent<Animator>();
    }

    private void Update()
    {
      SetupAnimation();
    }

    private void SetupAnimation()
    {
      animator.SetBool("IsWalking", movementCtrl.IsWalking);
      animator.SetFloat("Direction", movementCtrl.Direction);
      animator.SetFloat("Speed", Mathf.Abs(movementCtrl.CurrentVelocity));
    }
  }

}
