using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button gameQuitButton;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(() => SceneSwitcher.LoadScene(SceneSwitcher.Scene.GameScene));
        gameQuitButton.onClick.AddListener(() => Application.Quit());
    }
}
