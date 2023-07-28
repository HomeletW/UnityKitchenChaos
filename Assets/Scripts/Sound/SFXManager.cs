using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private const string PLAYER_PREF_SFX_VOLUME = "SFXVolumeLevel";

    public event EventHandler<int> OnSFXVolumeChanged;

    public static SFXManager Instance { get; private set; }

    [SerializeField] private SFXSO sfx;
    [SerializeField] private int defaultVolumeLevel = 10;
    [SerializeField] private int maxVolumeLevel = 10;

    private bool movingSync = true;   // true on left foot, false on right foot
    private int MovingSFXIndex
    {
        get
        {
            movingSync = !movingSync;
            return movingSync ? 0 : 1;
        }
    }

    private int volumeLevel;
    private float Volume => volumeLevel * 0.1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Cannot have mutiple instance of SFXManager");
        }
        Instance = this;
        volumeLevel = PlayerPrefs.GetInt(PLAYER_PREF_SFX_VOLUME, defaultVolumeLevel);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderDelivered += (obj, _) => PlaySFX(sfx.deliverySuccess, DeliveryManager.Instance.transform.position);
        DeliveryManager.Instance.OnFailedOrderDeliver += (obj, _) => PlaySFX(sfx.deliveryFail, DeliveryManager.Instance.transform.position);
        CuttingCounter.OnAnyCut += (obj, _) => PlaySFX(sfx.chop, (obj as CuttingCounter).transform.position);
        Player.Instance.OnPlayerPickedUp += (obj, _) => PlaySFX(sfx.objectPickup, (obj as Player).transform.position);
        PlateKitchenObject.OnAnyIngredientAdded += (obj, _) => PlaySFX(sfx.objectPickup, (obj as PlateKitchenObject).transform.position);
        HolderCounter.OnAnyItemPlaced += (obj, _) => PlaySFX(sfx.objectDrop, (obj as HolderCounter).transform.position);
        TrashCounter.OnAnyItemTrashed += (obj, _) => PlaySFX(sfx.trash, (obj as TrashCounter).transform.position);
    }

    public void PlayWarningSound(Vector3 position, float volumeMultipler = 1f)
    {
        PlaySFX(sfx.warn[1], position, volumeMultipler);
    }

    public void PlayCountdownSound(float volumeMultipler = 1f)
    {
        PlaySFX(sfx.warn[0], Camera.main.transform.position, volumeMultipler);
    }

    public void PlayWalkingSound(Vector3 position, float volumeMultipler = 1f)
    {
        PlaySFX(sfx.walk[MovingSFXIndex], position, volumeMultipler);
    }

    public void PlaySprintingSound(Vector3 position, float volumeMultipler = 1f)
    {
        PlaySFX(sfx.sprint[MovingSFXIndex], position, volumeMultipler);
    }

    private void PlaySFX(AudioClip[] clip, Vector3 position, float volumeMultipler = 1f)
    {
        PlaySFX(clip[UnityEngine.Random.Range(0, clip.Length)], position, volumeMultipler);
    }

    private void PlaySFX(AudioClip clip, Vector3 position, float volumeMultipler = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, position, Volume * volumeMultipler);
    }

    public void IncreaseVolumeLevel()
    {
        volumeLevel = (volumeLevel + 1) % (maxVolumeLevel + 1);
        OnSFXVolumeChanged?.Invoke(this, volumeLevel);
        PlayerPrefs.SetInt(PLAYER_PREF_SFX_VOLUME, volumeLevel);
    }

    public int GetVolumeLevel()
    {
        return volumeLevel;
    }
}
