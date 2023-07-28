using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button goToMainMenuButton;
    [SerializeField] private Button showOptionsButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(ResumeAction);
        goToMainMenuButton.onClick.AddListener(GoToMainMenuAction);
        showOptionsButton.onClick.AddListener(ShowOptionsAction);
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        gameObject.SetActive(false);
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.GameStateChangedArg e)
    {
        if (e.newState == GameManager.State.GAME_PAUSING)
        {
            gameObject.SetActive(true);
            resumeButton.Select();
        }
        else if (e.lastState == GameManager.State.GAME_PAUSING)
        {
            // just switched out from pausing
            gameObject.SetActive(false);
            OptionMenuUI.Instance.Hide(skipAction: true);
        }
    }

    private void ResumeAction()
    {
        GameManager.Instance.ExitPause();
    }

    private void GoToMainMenuAction()
    {
        GameManager.Instance.ExitPause(() => SceneSwitcher.LoadScene(SceneSwitcher.Scene.MainMenu));
    }

    private void ShowOptionsAction()
    {
        gameObject.SetActive(false);
        OptionMenuUI.Instance.Show(hideAction: () =>
        {
            gameObject.SetActive(true);
            resumeButton.Select();
        });
    }
}
