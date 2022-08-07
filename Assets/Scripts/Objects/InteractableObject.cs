using UnityEngine;
using TheLonelyOne.GUI.Icon;

namespace TheLonelyOne
{
  public class InteractableObject : MonoBehaviour, IInteractable
  {
    #region COMPOMEMTS
    protected GameObject icon;
    #endregion

    #region IInteractable
    public virtual void Interact()
    {
      // Empty
    }
    #endregion

    protected virtual void Awake()
    {
      icon = transform.Find("Icon").gameObject;

      enabled = false;
    }

    protected void OnTriggerEnter2D(Collider2D _collision)
    {
      if (icon)
        icon.SetActive(true);
    }

    protected void OnTriggerExit2D(Collider2D _collision)
    {
      if (icon)
        icon.SetActive(false);
    }

  }
}
