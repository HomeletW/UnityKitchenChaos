using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float maxCountdownTextDegAbs = 15f;
    [SerializeField] private float minCountdownTextDegAbs = 5f;

    bool countdownRunning = false;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        countdownText.gameObject.SetActive(false);
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.GameStateChangedArg e)
    {
        if (e.newState == GameManager.State.GAME_START_COUNTDOWN && !countdownRunning)
        {
            StartCoroutine(StartTimer());
        }
    }

    private IEnumerator StartTimer()
    {
        countdownRunning = true;
        yield return null;   // wait for the end of frame to sync up
        countdownText.gameObject.SetActive(true);
        while (!GameManager.Instance.GameStartCountdownEnded)
        {
            int countdown = GameManager.Instance.GameStartCountdownFloored;
            if (countdown == 0)
            {
                SyncCountdownText("Go!");
            }
            else
            {
                SyncCountdownText(countdown.ToString());
            }
            yield return null;
        }
        countdownText.gameObject.SetActive(false);
        countdownRunning = false;
    }

    private void SyncCountdownText(string newText)
    {
        if (newText != countdownText.text)
        {
            countdownText.text = newText;
            float absRange = maxCountdownTextDegAbs - minCountdownTextDegAbs;
            float angle = Random.Range(0, 2 * absRange) - absRange;
            if (angle > 0)
            {
                angle += minCountdownTextDegAbs;
            }
            else
            {
                angle -= minCountdownTextDegAbs;
            }
            countdownText.transform.eulerAngles = new Vector3(0, 0, angle);
            SFXManager.Instance.PlayCountdownSound();
        }
    }
}

