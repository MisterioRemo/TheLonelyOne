using UnityEngine;

namespace TheLonelyOne.UI.Icon
{
  [RequireComponent(typeof(Animator))]
  public class InteractableIcon : MonoBehaviour
  {
    #region PARAMETERS
    private Animator animator;

    [SerializeField] private InteractableIconType iconType;
    #endregion

    #region PROPERTIES
    public InteractableIconType IconType { get => iconType; }
    #endregion

    private void Awake()
    {
      animator = GetComponent<Animator>();
      gameObject.SetActive(false);
    }

    private void OnEnable()
    {
      if (animator)
        animator.SetInteger("State", (int)iconType);
    }
  }
}
