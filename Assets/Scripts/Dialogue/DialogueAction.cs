using System.Reflection;
using UnityEngine;
using Zenject;

namespace TheLonelyOne.Dialogue
{
  public class DialogueAction
  {
    [Inject] protected Player.PlayerController playerCtrl;
    [Inject] protected DialogueManager         dialogueManager;

    public void Invoke(string _methodName, params string[] _params)
    {
      MethodInfo method = typeof(DialogueAction).GetMethod(_methodName);
      method?.Invoke(this, _params);
    }

    public void SetPosition(string _targetName, string _position)
    {
      SetPositionImplementation(dialogueManager.Parser.ParseFuctionTarget(_targetName),
                                Utils.ParseToVector3(_position));
    }

    public void SetPositionImplementation(GameObject _gameObject, Vector3 _position)
    {
      _gameObject.transform.position = _position;
    }

    public void ShowUI(string _canvasName)
    {
      GameManager.Instance.UIObjects.TryGetValue(_canvasName, out GameObject canvas);

      if (canvas == null)
      {
        Debug.LogError($"Can't find '{_canvasName}' ui object in GameManager.Instance.UIObjects.");
        return;
      }

      playerCtrl.ChangeInputActionsMap(Player.InputActionsMap.UI);
      canvas.SetActive(true);
    }
  }
}
