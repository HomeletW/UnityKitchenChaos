using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : KitchenCounter
{
    /// <summary>
    /// Event arg is an offset, indicating the number of plates that was spawned (positive) or despawned (negative)
    /// </summary>
    public event EventHandler<int> OnPlatesCountUpdated;

    [SerializeField] private KitchenObjectSO plateSO;
    [SerializeField] private int maxPlatesCount = 5;
    [SerializeField] private float platesSpawnTime = 4f;

    private int platesCount = 0;
    private float spawnTimer = 0f;

    private bool CanSpawnPlate => platesCount < maxPlatesCount && GameManager.Instance.IsPlaying;
    private bool HasPlate => platesCount > 0;
    private bool SpawnTimerFinished => spawnTimer >= platesSpawnTime;

    private void Awake()
    {
        // begin with timer finished, so that one plate is spawned right away
        spawnTimer = platesSpawnTime;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.GameStateChangedArg e)
    {
        if (e.newState == GameManager.State.GAME_START_COUNTDOWN)
        {
            ResetPlates();
        }
    }

    private void Update()
    {
        if (CanSpawnPlate)
        {
            spawnTimer += Time.deltaTime;
            if (SpawnTimerFinished)
            {
                SpawnPlate();
                spawnTimer = 0;
            }
        }
    }

    protected override void InteractAction(Player player)
    {
        if (!HasPlate)
        {
            return;
        }

        if (player.HoldingObject)
        {
            // try to take a plate and place the object on top

            // spawns a dummy plate to try placing the object
            PlateKitchenObject plate = KitchenObject.Spawn(plateSO, null) as PlateKitchenObject;
            if (player.GetCurrentKitchenObject().AddToPlate(plate))
            {
                // if successful, player holds that plate and decrease the plate count
                plate.SetHolder(player);
                SetPlateCount(platesCount - 1);
            }
            else
            {
                // if not succeed, destroy the dummy plate and player is not touched
                plate.DestroySelf();
            }
        }
        else
        {
            // not holding an object simply takes a plate
            KitchenObject.Spawn(plateSO, player);
            SetPlateCount(platesCount - 1);
        }
    }

    private void SpawnPlate()
    {
        SetPlateCount(platesCount + 1);
    }

    private void SetPlateCount(int newPlateCount)
    {
        platesCount = newPlateCount;
        OnPlatesCountUpdated?.Invoke(this, newPlateCount);
    }

    private void ResetPlates()
    {
        platesCount = 0;
        spawnTimer = 0f;
        OnPlatesCountUpdated?.Invoke(this, 0);
    }
}
