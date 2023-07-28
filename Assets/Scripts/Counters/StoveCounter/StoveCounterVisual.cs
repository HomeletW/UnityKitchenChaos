using UnityEngine;
using System.Collections;

public class StoveCounterVisual : MonoBehaviour
{
    private StoveCounter stoveCounter;

    [SerializeField] GameObject[] stoveOnEffect;

    private void Awake()
    {
        stoveCounter = GetComponentInParent<StoveCounter>();
    }

    private void Start()
    {
        stoveCounter.OnStoveStateUpdate += StoveCounter_OnStoveStateUpdate;
    }

    private void StoveCounter_OnStoveStateUpdate(object sender, StoveCounter.StoveStateUpdateArg e)
    {
        foreach (GameObject effectObj in stoveOnEffect)
        {
            effectObj.SetActive(e.IsStoveActive());
        }
    }
}

