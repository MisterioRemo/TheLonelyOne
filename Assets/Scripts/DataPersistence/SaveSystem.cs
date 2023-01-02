using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using System;

namespace TheLonelyOne
{
  public class SaveSystem : MonoBehaviour
  {
    #region PARAMETERS
    private string                 fileName;
    private bool                   useEncryption;
    private GameData               gameData               = new GameData();
    private List<IDataPersistence> dataPersistenceObjects = new List<IDataPersistence>();
    private PersistentDataHandler  persistentDataHandler;
    #endregion

    [Inject]
    public void Init(string _fileName, bool _useEncryption)
    {
      fileName      = _fileName;
      useEncryption = _useEncryption;
    }

    #region LIFECYCLE
    private void Start()
    {
      persistentDataHandler  = new PersistentDataHandler(Application.persistentDataPath, fileName, useEncryption);
      dataPersistenceObjects = Utils.Concat(dataPersistenceObjects, FindAllDataPersistenceMonoObjects());
      LoadGame();
    }

    private void OnApplicationQuit()
    {
      SaveGame();
    }
    #endregion

    #region INTREFACE
    public void NewGame()
    {
      gameData = new GameData();
    }

    public void LoadGame()
    {
      gameData = persistentDataHandler.Load();

      if (gameData == null)
        NewGame();

      foreach (var dpObject in dataPersistenceObjects)
        dpObject.Load(gameData);
    }

    public void SaveGame()
    {
      foreach (var dpObject in dataPersistenceObjects)
        dpObject.Save(ref gameData);

      persistentDataHandler.Save(gameData);
    }

    public void AddDataPersistenceObject(IDataPersistence _object)
    {
      dataPersistenceObjects.Add(_object);
    }

    public void LoadDataPersistenceObject(IDataPersistence _object)
    {
      _object.Load(gameData);
    }
    #endregion

    #region METHODS
    private void FocusChangedCallback(bool _hasFocus)
    {
      if (!_hasFocus)
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceMonoObjects()
    {
      return FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>().ToList();
    }
    #endregion
  }
}
