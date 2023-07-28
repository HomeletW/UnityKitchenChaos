using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREF_MUSIC_VOLUME = "MusicVolumeLevel";

    public event EventHandler<int> OnMusicVolumeChanged;

    public static MusicManager Instance { get; private set; }

    [SerializeField] private int defaultVolumeLevel = 3;
    [SerializeField] private int maxVolumeLevel = 10;

    private AudioSource source;

    private int volumeLevel = 3;
    private float Volume => volumeLevel * 0.1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Cannot have multiple instance of MusicManager");
        }
        Instance = this;

        source = GetComponent<AudioSource>();
        volumeLevel = PlayerPrefs.GetInt(PLAYER_PREF_MUSIC_VOLUME, defaultVolumeLevel);
        source.volume = Volume;
    }

    public void IncreaseVolumeLevel()
    {
        volumeLevel = (volumeLevel + 1) % (maxVolumeLevel + 1);
        source.volume = Volume;
        OnMusicVolumeChanged?.Invoke(this, volumeLevel);
        PlayerPrefs.SetInt(PLAYER_PREF_MUSIC_VOLUME, volumeLevel);
    }

    public int GetVolumeLevel()
    {
        return volumeLevel;
    }
}
