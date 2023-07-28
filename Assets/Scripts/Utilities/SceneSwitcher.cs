using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSwitcher
{
    public enum Scene
    {
        GameScene,
        LoadingScene,
        MainMenu,
    }

    public static Scene TargetScene { get; internal set; }

    public static void LoadScene(Scene targetScene)
    {
        TargetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
}
