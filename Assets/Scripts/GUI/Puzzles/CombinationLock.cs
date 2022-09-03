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

    [ContextMenu("Generate guid fo id")]
    protected void GenerateGuid() => id = System.Guid.NewGuid().ToString();

    #region IDataPersistence
    public void Save(ref GameData _gameData)
    {
      if (!String.IsNullOrEmpty(currentCombination))
        _gameData.CombinationLocks[id] = currentCombination;
    }

    public void Load(GameData _gameData)
    {
      _gameData.CombinationLocks.TryGetValue(id, out string combination);

      if (!String.IsNullOrEmpty(combination))
        currentCombination = combination;
    }
    #endregion

    protected void Awake()
    {
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
        GameEvents.Instance.CompletePuzzle(id);
    }
  }
}
