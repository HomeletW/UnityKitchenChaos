using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private KitchenCounter counter;

    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.OnSelectedCounterChange += Player_CounterChange;
        counter = GetComponentInParent<KitchenCounter>();
    }

    private void Player_CounterChange(object sender, Player.OnSelectedCounterChangeEventArg e)
    {
        SetSelectedVisual(counter == e.selected);
    }

    private void SetSelectedVisual(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}
