using UnityEngine;

namespace TheLonelyOne
{
  public class InteractableObject : MonoBehaviour, IInteractable
  {
    #region COMPONENTS
    [SerializeField] protected GameObject icon;
    #endregion

    #region IInteractable
    public virtual void Interact()
    {
      // Empty
    }
    #endregion

    protected virtual void Awake()
    {
      enabled = false;
    }

    protected void OnTriggerEnter2D(Collider2D _collision)
    {
      if (icon && _collision.gameObject.tag == "Player")
        icon.SetActive(true);
    }

    protected void OnTriggerExit2D(Collider2D _collision)
    {
      if (icon && _collision.gameObject.tag == "Player")
        icon.SetActive(false);
    }

  }
}
