using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BindingGroup : MonoBehaviour
{
    [SerializeField] private GameObject rebindPrompt;
    [SerializeField] private Button bindingButton;
    [SerializeField] private GameInput.Binding binding;

    private TextMeshProUGUI bindingText;

    private void Awake()
    {
        bindingText = bindingButton.GetComponentInChildren<TextMeshProUGUI>();
        bindingButton.onClick.AddListener(Rebind);
    }

    private void Start()
    {
        UpdateBindingText();
    }

    private void UpdateBindingText()
    {
        bindingText.text = GameInput.Instance.GetKeyboardBindingText(binding);
    }

    private void Rebind()
    {
        rebindPrompt.SetActive(true);
        GameInput.Instance.InteractiveRebind(binding, () =>
        {
            rebindPrompt.SetActive(false);
            UpdateBindingText();
        });
    }
}
