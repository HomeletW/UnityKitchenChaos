using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";

    private Animator animator;
    private CuttingCounter cuttingCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cuttingCounter = GetComponentInParent<CuttingCounter>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, CuttingCounter.CutProgressUpdatedArg e)
    {
        if (e.active)
        {
            animator.SetTrigger(CUT);
        }
    }
}
