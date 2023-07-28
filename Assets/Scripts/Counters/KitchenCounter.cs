using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of all kitchen counter
/// </summary>
public class KitchenCounter : MonoBehaviour
{
    public event EventHandler<Player> OnPlayerInteracted;
    public event EventHandler<Player> OnPlayerInteractedAlt;

    [SerializeField] protected Transform spawnPoint;

    public void Interact(Player player)
    {
        InteractAction(player);
        OnPlayerInteracted?.Invoke(this, player);
    }

    public void InteractAlt(Player player)
    {
        InteractAltAction(player);
        OnPlayerInteractedAlt?.Invoke(this, player);
    }

    protected virtual void InteractAction(Player player) { }

    protected virtual void InteractAltAction(Player player) { }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }
}
