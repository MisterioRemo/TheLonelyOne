using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TheLonelyOne
{
  public class InteractableObject : MonoBehaviour, IInteractable
  {
    #region PARAMETERS
    private GameObject icon;

    [Inject] protected Player.PlayerController playerCtrl;

    [Header("Data Persistence")]
    [SerializeField] protected string           id;
    [SerializeField] protected List<GameObject> childObjectToSave;
    #endregion

    #region PROPERTIES
    public bool IconVisability { get => icon ? icon.activeSelf : false;
                                 set
                                 {
                                   if (icon)
                                     icon.SetActive(value);
                                 }
                               }
    #endregion

    [ContextMenu("Generate guid fo id")]
    protected void GenerateGuid() => id = Utils.GenerateGuid();

    #region IINTERACTABLE
    public virtual void Interact()
    {
      // Empty
    }
    #endregion

    #region IDataPersistence
    public virtual void Save(ref GameData _gameData)
    {
      if (string.IsNullOrEmpty(id))
      {
        Debug.LogError($"Can't save '{name}' object: id is null or empty.");
        return;
      }

      ObjectData data = SaveObject(gameObject);
      data.Children   = new SerializableTypes.SerializableObjectDictionary();

      foreach (var child in childObjectToSave)
        data.Children.Add(child.name, SaveObject(child));

      _gameData.Objects[id] = data;
    }

    protected virtual ObjectData SaveObject(GameObject _target)
    {
      ObjectData data = new ObjectData();

      data.Position   = _target.transform.position;
      data.IsActive   = _target.activeSelf;
      data.State      = SaveObjectState(_target);
      data.Children   = null;

      return data;
    }

    protected virtual ObjectStateData SaveObjectState(GameObject _target)
    {
      return null;
    }

    public virtual void Load(GameData _gameData)
    {
      _gameData.Objects.TryGetValue(id, out ObjectData data);

      if (data == null)
        return;

      LoadObject(gameObject, data);
    }

    protected virtual void LoadObject(GameObject _target, ObjectData _data)
    {
      _target.transform.position = _data.Position;
      _target.SetActive(_data.IsActive);

      LoadObjectState(_target, _data.State);

      if (_data.Children != null)
        foreach (var child in _data.Children)
          LoadObject(gameObject.transform.Find(child.Key).gameObject, child.Value);
    }

    protected virtual void LoadObjectState(GameObject _target, ObjectStateData _state)
    {
      // Empty
    }
    #endregion

    #region LIFECYCLE
    protected virtual void Awake()
    {
      icon = transform.Find("Icon").gameObject;
    }

    protected virtual void OnDestroy()
    {
      playerCtrl.OnInteractableSet -= InteractableSetCallback;
    }
    #endregion

    #region COLLISIONS
    protected virtual void OnTriggerEnter2D(Collider2D _collision)
    {
      if (_collision.CompareTag("Player"))
      {
        IconVisability                = true;
        playerCtrl.OnInteractableSet += InteractableSetCallback;
      }
    }

    protected virtual void OnTriggerExit2D(Collider2D _collision)
    {
      if (_collision.CompareTag("Player"))
      {
        IconVisability                = false;
        playerCtrl.OnInteractableSet -= InteractableSetCallback;
      }
    }
    #endregion

    #region METHODS
    private void InteractableSetCallback(IInteractable _interactable)
    {
      IconVisability = (IInteractable)this == _interactable;
    }
    #endregion
  }
}
