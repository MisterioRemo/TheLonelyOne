using System.Reflection;
using UnityEngine;
using Zenject;

namespace TheLonelyOne.Dialogue
{
  public partial class DialogueManager
  {
    public class DialogueAction
    {
      protected readonly DialogueManager  parent;

      [Inject] protected UIObjectsManager uiManager;
      [Inject] protected PlotManager      plotManager;

      public DialogueAction(DialogueManager _dialogueManager)
      {
        parent = _dialogueManager;
      }

      public void Invoke(string _methodName, params string[] _params)
      {
        MethodInfo method = typeof(DialogueAction).GetMethod(_methodName);
        method?.Invoke(this, _params);
      }

      public void SetPosition(string _targetName, string _position)
      {
        SetPositionImplementation(parent.Parser.ParseFuctionTarget(_targetName),
                                  TheLonelyOne.Utils.ParseToVector3(_position));
      }

      public void SetPositionImplementation(GameObject _gameObject, Vector3 _position)
      {
        _gameObject.transform.position = _position;
      }

      public void ShowUI(string _canvasName)
      {
        uiManager.UIObjects.TryGetValue(_canvasName, out GameObject canvas);

        if (canvas == null)
        {
          Debug.LogError($"Can't find '{_canvasName}' ui object in GameManager.Instance.UIObjects.");
          return;
        }

        parent.playerCtrl.ChangeInputActionsMap(Player.InputActionsMap.UI);
        canvas.SetActive(true);
      }

      public void CompletePlotPoint(string _name)
      {
        if (plotManager.FindPlotPoint(_name) is Goal.PlotPoint plotPoint)
        {
          plotPoint.Complete();
          return;
        }

        plotManager.AddAchievedPlotPoint(_name);
      }

      public void RemoveAchievedPlotPoint(string _name)
      {
        if (plotManager.FindPlotPoint(_name) is IReversible plotPoint)
        {
          plotPoint.Undo();
          return;
        }

        plotManager.RemoveAchievedPlotPoint(_name);
      }

      public void Clear()
      {
        parent.Narration.Clear();
      }
    }
  }
}
