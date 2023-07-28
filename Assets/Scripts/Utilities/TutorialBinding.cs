using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialBinding : MonoBehaviour
{
    [SerializeField] private GameInput.Binding binding;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        text.text = GameInput.Instance.GetKeyboardBindingText(binding);
    }
}
