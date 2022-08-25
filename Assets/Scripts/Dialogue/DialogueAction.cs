using System.Reflection;
using UnityEngine;

namespace TheLonelyOne.Dialogue
{
  public class DialogueAction
  {
    public static void Invoke(GameObject _gameObject, string _methodName, params string[] _params)
    {
      object[] parameters = new object[_params.Length + 1];
      parameters[0] = _gameObject;
      _params.CopyTo(parameters, 1);

      MethodInfo method = typeof(DialogueAction).GetMethod(_methodName);
      method.Invoke(null, parameters);
    }

    public static void SetPosition(GameObject _gameObject, string _param)
    {
      SetPositionImplementation(_gameObject, Utils.ParseToVector3(_param));
    }

    public static void SetPositionImplementation(GameObject _gameObject, Vector3 _position)
    {
      _gameObject.transform.position = _position;
    }
  }
}
