using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuUI : MonoBehaviour
{
    public static OptionMenuUI Instance { get; private set; }

    [SerializeField] private Button sfxVolumnButton;
    [SerializeField] private Button musicVolumnButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private string sfxVolumeFormatText = "SFX Volume: {0}";
    [SerializeField] private string musicVolumeFormatText = "Music Volume: {0}";

    private TextMeshProUGUI sfxVolumeText;
    private TextMeshProUGUI musicVolumeText;

    private Action hideAction = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Cannot have multiple OptionUI");
        }
        Instance = this;

        sfxVolumeText = sfxVolumnButton.GetComponentInChildren<TextMeshProUGUI>();
        musicVolumeText = musicVolumnButton.GetComponentInChildren<TextMeshProUGUI>();

        sfxVolumnButton.onClick.AddListener(() => SFXManager.Instance.IncreaseVolumeLevel());
        musicVolumnButton.onClick.AddListener(() => MusicManager.Instance.IncreaseVolumeLevel());
        closeButton.onClick.AddListener(() => Hide());
    }

    private void Start()
    {
        SFXManager.Instance.OnSFXVolumeChanged += (_, newVolumeLevel) => SetSFXVolumeText(newVolumeLevel);
        MusicManager.Instance.OnMusicVolumeChanged += (_, newVolumeLevel) => SetMusicVolumeText(newVolumeLevel);

        SetSFXVolumeText(SFXManager.Instance.GetVolumeLevel());
        SetMusicVolumeText(MusicManager.Instance.GetVolumeLevel());

        Hide();
    }

    private void SetSFXVolumeText(int volume)
    {
        sfxVolumeText.text = string.Format(sfxVolumeFormatText, volume);
    }

    private void SetMusicVolumeText(int volume)
    {
        musicVolumeText.text = string.Format(musicVolumeFormatText, volume);
    }

    public void Show(Action hideAction = null)
    {
        gameObject.SetActive(true);
        this.hideAction = hideAction;
        sfxVolumnButton.Select();
    }

    public void Hide(bool skipAction = false)
    {
        gameObject.SetActive(false);
        if (!skipAction)
        {
            hideAction?.Invoke();
        }
        hideAction = null;
    }
}
