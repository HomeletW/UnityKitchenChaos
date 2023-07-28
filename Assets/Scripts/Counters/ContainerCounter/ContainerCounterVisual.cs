using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    private Animator animator;
    private ContainerCounter containerCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        containerCounter = GetComponentInParent<ContainerCounter>();
    }

    private void Start()
    {
        containerCounter.OnPlayerInteracted += ContainerCounter_OnPlayerInteracted;
    }

    private void ContainerCounter_OnPlayerInteracted(object sender, Player e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
