using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private TutorialBinding[] bindings;

    private void Awake()
    {
        bindings = GetComponentsInChildren<TutorialBinding>();
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        GameInput.Instance.OnPlayerInteract += GameInput_OnPlayerInteract;
        Hide();
    }

    private void GameInput_OnPlayerInteract(object sender, System.EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            GameManager.Instance.ExitTutorial();
        }
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.GameStateChangedArg e)
    {
        if (e.newState == GameManager.State.GAME_START_WAITING)
        {
            Show();
        }
        else if (e.lastState == GameManager.State.GAME_START_WAITING)
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        foreach (TutorialBinding binding in bindings)
        {
            binding.UpdateVisual();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
