using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameRunningStatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeRemainingText;

    private bool timerRunning = false;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        timeRemainingText.gameObject.SetActive(false);
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.GameStateChangedArg e)
    {
        if (e.newState == GameManager.State.GAME_RUNNING && !timerRunning)
        {
            StartCoroutine(StartTimer());
        }
    }

    private IEnumerator StartTimer()
    {
        timerRunning = true;
        timeRemainingText.gameObject.SetActive(true);
        timeRemainingText.text = "";
        yield return null;   // wait for the end of frame to sync up
        while (!GameManager.Instance.GameRunningCountdownEnded)
        {
            int countdown = GameManager.Instance.GameRunningCountdownFloored;
            timeRemainingText.text = GetCountdownText(countdown);
            yield return null;
        }
        timerRunning = false;
        timeRemainingText.gameObject.SetActive(false);
    }

    private string GetCountdownText(int seconds)
    {
        if (seconds == 0)
        {
            return "Time's Up!";
        }
        else
        {
            int minutes = seconds / 60;
            int secondsRemainder = seconds % 60;
            return string.Format("Time remaining {0}:{1}", minutes, secondsRemainder.ToString().PadLeft(2, '0'));
        }
    }
}
