using UnityEngine;

namespace TheLonelyOne
{
  public class UIState : MonoBehaviour, IDataPersistence
  {
    #region PARAMETERS
    [SerializeField] protected string id;
    #endregion

    [ContextMenu("Generate guid fo id")]
    protected void GenerateGuid() => id = Utils.GenerateGuid();

    #region IDataPersistence
    public virtual void Save(ref GameData _gameData)
    {
      // Empty
    }

    public virtual void Load(GameData _gameData)
    {
      // Empty
    }
    #endregion

    #region LIFECYCLE
    protected virtual void Awake()
    {
      enabled = false;
    }
    #endregion
  }
}
