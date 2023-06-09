using UnityEngine;

namespace TheLonelyOne
{
  [RequireComponent(typeof(UnityEngine.UI.Image))]
  public class UISpriteState : UIState
  {
    #region PARAMETERS
    [SerializeField] protected Sprite[] sprites;
    [SerializeField] protected int      spriteId;

    protected UnityEngine.UI.Image imageComponent;
    #endregion

    #region PROPERTIES
    public int SpriteId { get => spriteId;
                          set {
                            spriteId = Mathf.Clamp(value, 0, sprites.Length - 1);

                            if (spriteId != value)
                              Debug.LogError($"IndexOutOfRangeException in '{name}' UI object.");
                          }
                        }
    #endregion

    #region IDataPersistence
    public override void Save(ref GameData _gameData)
    {
      _gameData.UIObjects[id] = new UIData {
        IsActive = gameObject.activeSelf,
        State    = new UISpriteData { SpriteId = SpriteId }
      };
    }

    public override void Load(GameData _gameData)
    {
      _gameData.UIObjects.TryGetValue(id, out UIData data);

      if (data != null && data.State is UISpriteData spriteState)
      {
        SpriteId                                            = spriteState.SpriteId;
        GetComponent<UnityEngine.UI.Image>().overrideSprite = sprites[SpriteId];

        gameObject.SetActive(data.IsActive);
      }
    }
    #endregion

    #region LIFECYCLE
    protected override void Awake()
    {
      base.Awake();
      imageComponent = GetComponent<UnityEngine.UI.Image>();
    }
    #endregion

    #region INTERFACE
    public void SetSprite(int _id)
    {
      SpriteId                      = _id;
      imageComponent.overrideSprite = sprites[SpriteId];
    }
    #endregion
  }
}
