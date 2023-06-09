using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheLonelyOne.UI
{
  public class CombinationLock : MonoBehaviour, IDataPersistence
  {
    #region PARAMETERS
    [SerializeField] protected string id;
    [SerializeField] protected string rightCombination;

    [RequireInterface(typeof(ICombinationLockElement))]
    [SerializeField] protected List<GameObject> lockElementObjects;

    protected List<ICombinationLockElement> lockElements;
    protected string                        currentCombination;
    #endregion

    #region PROPERTIES
    public bool IsCombinationValid { get; private set; }
    #endregion

    #region EVENTS
    public event Action<string> OnPuzzleCompleted;
    #endregion

    [ContextMenu("Generate guid fo id")]
    protected void GenerateGuid() => id = Utils.GenerateGuid();

    #region IDataPersistence
    public void Save(ref GameData _gameData)
    {
      if (!String.IsNullOrEmpty(currentCombination))
      {
        _gameData.CombinationLocks[id]                 = new CombinationLockData();
        _gameData.CombinationLocks[id].LastCombination = currentCombination;
        _gameData.CombinationLocks[id].IsActive        = gameObject.activeSelf;

      }
    }

    public void Load(GameData _gameData)
    {
      _gameData.CombinationLocks.TryGetValue(id, out CombinationLockData combination);

      if (combination != null)
      {
        currentCombination = combination.LastCombination;
        gameObject.SetActive(combination.IsActive);
      }

    }
    #endregion

    #region LIFECYCLE
    protected void Awake()
    {
      if (currentCombination == rightCombination)
        IsCombinationValid = true;

      bool firstInitialization = string.IsNullOrEmpty(currentCombination);

      lockElements = new List<ICombinationLockElement>();

      for (int i = 0; i < lockElementObjects.Count; ++i)
      {
        lockElements.Add(lockElementObjects[i].GetComponent<ICombinationLockElement>());
        lockElements[i].OnSymbolChange += ValidateCombination;

        if (firstInitialization)
          currentCombination += lockElements[i].Symbol;
        else
          lockElements[i].Symbol = currentCombination[i];
      }
    }

    protected void OnDestroy()
    {
      foreach (var element in lockElements)
        if (element != null)
          element.OnSymbolChange -= ValidateCombination;
    }
    #endregion

    #region INTERFACE
    public void ValidateCombination()
    {
      if (lockElements.Count != rightCombination.Length)
      {
        IsCombinationValid = false;
        return;
      }

      currentCombination = "";
      IsCombinationValid = true;

      for (int i = 0; i < rightCombination.Length; ++i)
      {
        currentCombination += lockElements[i].Symbol;

        if (lockElements[i].Symbol != rightCombination[i])
          IsCombinationValid = false;
      }

      if (IsCombinationValid)
        OnPuzzleCompleted?.Invoke(id);
    }
    #endregion
  }
}
