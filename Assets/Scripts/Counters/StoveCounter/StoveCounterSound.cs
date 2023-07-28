using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private float soundCheckInterval = 0.1f;

    private StoveCounter stoveCounter;
    private AudioSource audioSource;

    private bool stoveActive;

    private void Awake()
    {
        stoveCounter = GetComponentInParent<StoveCounter>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStoveStateUpdate += StoveCounter_OnStoveStateUpdate;
    }

    private void StoveCounter_OnStoveStateUpdate(object sender, StoveCounter.StoveStateUpdateArg e)
    {
        stoveActive = e.IsStoveActive();
        if (stoveActive)
        {
            audioSource.Play();
            if (e.isWarningRecipe)
            {
                StartCoroutine(PlayWarningSound());
            }
        }
        else
        {
            audioSource.Pause();
        }
    }

    private IEnumerator PlayWarningSound()
    {
        while (stoveActive)
        {
            float progress = stoveCounter.GetNormalizedProgress();
            if (progress > 0.5f)
            {
                SFXManager.Instance.PlayWarningSound(gameObject.transform.position);
            }
            yield return new WaitForSeconds(soundCheckInterval);
        }
    }
}
