using System.Reflection;
using UnityEngine;
using Zenject;

namespace TheLonelyOne.Dialogue
{
  public class DialogueAction
  {
    [Inject] protected static Player.PlayerController playerCtrl;

    public static void Invoke(string _methodName, params string[] _params)
    {
      MethodInfo method = typeof(DialogueAction).GetMethod(_methodName);
      method?.Invoke(null, _params);
    }

    public static void SetPosition(string _targetName, string _position)
    {
      SetPositionImplementation(DialogueManager.Parser.ParseFuctionTarget(_targetName),
                                Utils.ParseToVector3(_position));
    }

    public static void SetPositionImplementation(GameObject _gameObject, Vector3 _position)
    {
      _gameObject.transform.position = _position;
    }

    public static void ShowUI(string _canvasName)
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
