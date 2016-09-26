using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenesManager
{
    public delegate void LoadingProgressChanged(float progress);
    public delegate void LoadingDone();

    public ScenesManager() { }

    public void LoadScene(SceneName name, LoadingProgressChanged onProgressChangedHandler, LoadingDone onLoadingDoneHandler)
    {
        App.Instance.StartCoroutine(LoadSceneCoroutine(name, onProgressChangedHandler, onLoadingDoneHandler));
    }

    private IEnumerator LoadSceneCoroutine(SceneName name, LoadingProgressChanged onProgressChangedHandler, LoadingDone onLoadingDoneHandler)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name.ToString());

        while (!ao.isDone)
        {
            if (onProgressChangedHandler != null)
            {
                onProgressChangedHandler(ao.progress);
            }

            yield return null;
        }

        if (onLoadingDoneHandler != null)
        {
            onLoadingDoneHandler();
        }
    }
}