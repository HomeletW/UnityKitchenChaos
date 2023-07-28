using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryCountdownUI : MonoBehaviour
{
    [SerializeField] private Image countdownImage;

    private float maxCountdown = 0f;
    private float currentCountdown = 0f;

    private bool CountdownStarted => maxCountdown > 0;
    private bool CountdownFinished => currentCountdown >= maxCountdown;
    private float NormalizedProgress => Mathf.Min(currentCountdown / maxCountdown, 1f);

    private void Start()
    {
        DeliveryManager.Instance.OnCountdownStarted += Instance_OnCountdownStarted; ;
    }

    private void Instance_OnCountdownStarted(object sender, float nextOrderInterval)
    {
        StartCountdown(nextOrderInterval);
    }

    private void Update()
    {
        if (CountdownStarted)
        {
            // if countdown is valid, increment the countdown and set fillamount
            currentCountdown += Time.deltaTime;
            countdownImage.fillAmount = NormalizedProgress;
            if (CountdownFinished)
            {
                // on countdown finished, clear countdown
                ResetCountdown();
            }
        }
        else
        {
            // idle, do nothing
        }
    }

    private void StartCountdown(float seconds)
    {
        if (CountdownFinished)
        {
            maxCountdown = seconds;
            currentCountdown = 0f;
            countdownImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Cannot start a countdown when another is active");
        }
    }

    private void ResetCountdown()
    {
        maxCountdown = 0f;
        currentCountdown = 0f;
        countdownImage.gameObject.SetActive(false);
    }
}
