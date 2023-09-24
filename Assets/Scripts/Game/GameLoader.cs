using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace TheLonelyOne
{
  public class GameLoader : MonoBehaviour
  {
    #region PARAMETERS
    [Inject] protected DiContainer diContainer;
    #endregion

    #region LIFECYCLE
    protected void Awake()
    {
      DiContainerRef.Container = diContainer;
    }
    #endregion

    #region INTERFACE
    public void LoadScene(string _sceneName)
    {
      StartCoroutine(LoadSceneAsync(_sceneName));
    }
    #endregion

    #region METHODS
    private IEnumerator LoadSceneAsync(string _sceneName)
    {
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
      // TODO: loading screen

      while (!asyncLoad.isDone)
        yield return null;
    }
    #endregion
  }
}
