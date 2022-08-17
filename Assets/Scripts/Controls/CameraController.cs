using UnityEngine;

namespace TheLonelyOne
{
  public class CameraController : MonoBehaviour
  {
    #region COMPONENTS
    [SerializeField] private Rigidbody2D target;
    #endregion

    #region PARAMETERS
    [SerializeField] private float interpolationSpeed;
    [SerializeField] private float offset;
    #endregion

    void LateUpdate()
    {
      transform.position = Vector3.Lerp(transform.position,
                                        new Vector3(target.position.x + offset, transform.position.y, transform.position.z),
                                        interpolationSpeed * Time.deltaTime);
    }
  }
}
