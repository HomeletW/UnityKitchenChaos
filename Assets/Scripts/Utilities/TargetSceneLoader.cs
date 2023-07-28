using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetSceneLoader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadDestScene());
    }

    private IEnumerator LoadDestScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneSwitcher.TargetScene.ToString());
        while (!op.isDone)
        {
            yield return null;
        }
    }
}
