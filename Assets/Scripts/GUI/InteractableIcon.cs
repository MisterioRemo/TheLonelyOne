using UnityEngine;

namespace TheLonelyOne.GUI.Icon
{
  [RequireComponent(typeof(Animator))]
  public class InteractableIcon : MonoBehaviour
  {
    #region PARAMETERS
    [SerializeField] private InteractableIconType iconType;
    #endregion

    #region PROPERTIES
    public InteractableIconType IconType { get => iconType; }
    #endregion

    private void Awake()
    {
      GetComponent<Animator>().SetInteger("State", (int)iconType);
      gameObject.SetActive(false);
    }
  }
}
