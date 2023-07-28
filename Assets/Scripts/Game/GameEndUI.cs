using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI plateServedText;
    [SerializeField] private TextMeshProUGUI ingrUtilRateText;
    [SerializeField] private Button playAgainButton;

    [SerializeField] private string plateServedTextFormat;
    [SerializeField] private string ingrUtilRateTextFormat;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(PlayAgainAction);
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        HideEndUI();
    }

    private void PlayAgainAction()
    {
        SceneSwitcher.LoadScene(SceneSwitcher.Scene.MainMenu);
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.GameStateChangedArg e)
    {
        if (e.newState == GameManager.State.GAME_END)
        {
            ShowEndUI();
        }
        else
        {
            HideEndUI();
        }
    }

    private void ShowEndUI()
    {
        SetChildActive(true);
        plateServedText.text = string.Format(plateServedTextFormat, GameManager.Instance.PlateServed);
        ingrUtilRateText.text = string.Format(ingrUtilRateTextFormat, GameManager.Instance.IngredientUtilityPercent);
        playAgainButton.Select();
    }

    private void HideEndUI()
    {
        SetChildActive(false);
    }

    private void SetChildActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}
