using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TheLonelyOne.UI
{
  [RequireComponent(typeof(Button))]
  public class CloseButton : MonoBehaviour
  {
    #region PARAMETERS
    [SerializeField] protected GameObject targetUI;

    [Inject] protected Player.PlayerController playerCtrl;

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
      playerCtrl.ChangeInputActionsMap(Player.InputActionsMap.Player);
    }
  }
}
