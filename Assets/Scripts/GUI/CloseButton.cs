using UnityEngine;
using UnityEngine.UI;

namespace TheLonelyOne.UI
{
  [RequireComponent(typeof(Button))]
  public class CloseButton : MonoBehaviour
  {
    #region COMPONENTS
    [SerializeField] protected GameObject targetUI;
    #endregion

    #region PARAMETERS
    protected Button closeButton;
    #endregion

    protected void Awake()
    {
      enabled     = false;
      closeButton = GetComponent<Button>();

      closeButton.onClick.AddListener(CloseUI);
    }

    protected void OnDestroy()
    {
      closeButton.onClick.RemoveListener(CloseUI);
    }

    protected void CloseUI()
    {
      targetUI.SetActive(false);
      GameManager.Instance.PlayerController.ChangeInputActionsMap(Player.InputActionsMap.Player);
    }
  }
}
