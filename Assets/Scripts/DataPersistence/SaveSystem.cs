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
    protected string                 fileName;
    protected bool                   useEncryption;
    protected GameData               gameData;
    protected List<IDataPersistence> dataPersistenceObjects;
    protected PersistentDataHandler  persistentDataHandler;
    #endregion

    [Inject]
    public void Init(string _fileName, bool _useEncryption)
    {
      fileName      = _fileName;
      useEncryption = _useEncryption;
    }

    #region LIFECYCLE
    protected void Start()
    {
      gameData               = new GameData();
      persistentDataHandler  = new PersistentDataHandler(Application.persistentDataPath, fileName, useEncryption);
      dataPersistenceObjects = FindAllDataPersistenceObjects();
      LoadGame();
    }

    protected void OnApplicationQuit()
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
    #endregion

    #region METHODS
    protected void FocusChangedCallback(bool _hasFocus)
    {
      if (!_hasFocus)
        SaveGame();
    }

    protected List<IDataPersistence> FindAllDataPersistenceObjects()
    {
      return FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>().ToList();
    }
    #endregion
  }
}
