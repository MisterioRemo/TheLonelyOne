using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheLonelyOne
{
  public class SaveSystem : MonoBehaviour
  {
    public static SaveSystem Instance { get; private set; }

    #region PARAMETERS
    [SerializeField] private string fileName;
    [SerializeField] private bool   useEncryption;

    private GameData               gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private PersistentDataHandler  persistentDataHandler;
    #endregion

    private void Awake()
    {
      if (Instance != null && Instance != this)
      {
        Destroy(this);
        return;
      }

      Instance = this;
      gameData = new GameData();
    }

    private void Start()
    {
      persistentDataHandler  = new PersistentDataHandler(Application.persistentDataPath, fileName, useEncryption);
      dataPersistenceObjects = FindAllDataPersistenceObjects();
      LoadGame();
    }

    private void OnApplicationQuit()
    {
      SaveGame();
    }

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

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
      return FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>().ToList();
    }
  }
}
