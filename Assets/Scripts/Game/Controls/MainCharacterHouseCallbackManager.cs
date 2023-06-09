using UnityEngine;

namespace TheLonelyOne
{
  public class MainCharacterHouseCallbackManager : MonoBehaviour
  {
    #region PARAMETERS
    [SerializeField] protected UI.CombinationLock combinationLock;
    [SerializeField] protected int                strongboxSpriteId;
    #endregion

    #region LIFECYCLE
    protected virtual void Start()
    {
      combinationLock.OnPuzzleCompleted += OpenStrongbox;
    }

    protected virtual void OnDestroy()
    {
      combinationLock.OnPuzzleCompleted -= OpenStrongbox;
    }
    #endregion

    #region METHODS
    private void OpenStrongbox(string _id)
    {
      combinationLock.transform.parent.Find("Background").GetComponent<UISpriteState>().SetSprite(strongboxSpriteId);
      combinationLock.gameObject.SetActive(false);
    }
    #endregion
  }
}
