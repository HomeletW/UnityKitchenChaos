using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    private const string WARNING = "Warning";

    [SerializeField] private GameObject barGroup;
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject warnImage;
    [SerializeField] private GameObject progressTrackedGameObject;

    private Animator barAnimator;
    private IProgressTracked progressTracked;
    private bool isTracking;

    private void Awake()
    {
        if (!progressTrackedGameObject.TryGetComponent(out progressTracked))
        {
            Debug.LogError("GameObject " + progressTrackedGameObject + " is not IProgressTracked.");
        }
        barAnimator = barGroup.GetComponent<Animator>();
    }

    private void Start()
    {
        progressTracked.OnProgressChanged += ProgressTracked_OnProgressChanged;
        gameObject.SetActive(false);
    }

    private void ProgressTracked_OnProgressChanged(object sender, IProgressTracked.ProgressChangedArg e)
    {
        if (e.IsBarActive())
        {
            isTracking = true;
            gameObject.SetActive(true);
            StartCoroutine(TrackProgress(e.IsWarning()));
        }
        else
        {
            isTracking = false;
        }
    }

    private IEnumerator TrackProgress(bool isWarning)
    {
        warnImage.SetActive(isWarning);

        // start tracks
        while (isTracking)
        {
            float progress = progressTracked.GetNormalizedProgress();
            barImage.fillAmount = progress;
            if (isWarning && progress > 0.5f)
            {
                //Debug.Log("warning");
                barAnimator.SetBool(WARNING, true);
            }
            else
            {
                //Debug.Log("not warning");
                barAnimator.SetBool(WARNING, false);
            }
            yield return null;
        }

        // reset state
        gameObject.SetActive(false);
    }
}

