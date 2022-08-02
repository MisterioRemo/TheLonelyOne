using UnityEngine;
using TheLonelyOne.GUI.Icon;

namespace TheLonelyOne
{
  public class InteractableObject : MonoBehaviour, IInteractable
  {
    #region COMPOMEMTS
    private GameObject icon;
    #endregion

    #region IInteractable
    public void Interact()
    {
      // Empty
    }
    #endregion

    private void Awake()
    {
      icon = transform.Find("Icon").gameObject;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      icon.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
      icon.SetActive(false);
    }
  }
}
