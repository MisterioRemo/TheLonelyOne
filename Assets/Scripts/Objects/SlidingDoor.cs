using System.Collections.Generic;
using UnityEngine;

namespace TheLonelyOne
{
  public class SlidingDoor : InteractableObject
  {
    #region PARAMETERS
    [SerializeField] protected float         speed;
    [SerializeField] protected List<Vector3> destinationPoints;
    [SerializeField] protected float         epsilon;

    protected int     pointIndex;
    protected bool    isMoving;
    protected Vector3 direction;
    #endregion

    #region PROPERTIES
    public int PointIndex { get => pointIndex;
                            private set
                              {
                                if (value < pointIndex)
                                  return;
                                pointIndex = value >= destinationPoints.Count ? 0 : value;
                              }
                            }

    public Vector3 CurrentPoint { get => destinationPoints[PointIndex]; }
    #endregion

    #region INTERACTABLE OBJECT
    public override void Interact()
    {
      if (isMoving)
        return;

      base.Interact();

      PointIndex++;
      isMoving  = true;
      direction = (destinationPoints[PointIndex] - transform.position).normalized;
    }
    #endregion

    protected void LateUpdate()
    {
      if (isMoving)
      {
        if (Vector3.Distance(transform.position, destinationPoints[PointIndex]) < epsilon)
        {
          isMoving           = false;
          direction          = Vector3.zero;
          transform.position = destinationPoints[PointIndex];

          return;
        }

        transform.position += direction * speed * Time.deltaTime;
      }
    }
  }
}
