using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A type of counter, capable of spawning a certain type of kitchen object
/// when interacted with
/// </summary>
public class ContainerCounter : KitchenCounter
{
    public static event EventHandler<KitchenObjectSO> OnAnyObjectSpawned;

    public static void ResetStaticData()
    {
        OnAnyObjectSpawned = null;
    }

    [SerializeField] private KitchenObjectSO kitchenObject;

    protected override void InteractAction(Player player)
    {
        if (player.HoldingObject)
        {
            // check if holding a plate, if so directly adds to the plate
            if (player.GetCurrentKitchenObject().TryGetObject(out PlateKitchenObject plate))
            {
                if (plate.TryAddIngredient(kitchenObject))
                {
                    OnAnyObjectSpawned?.Invoke(this, kitchenObject);
                }
            }
        }
        else
        {
            // if not holding, directly spawn into player's hand
            KitchenObject.Spawn(kitchenObject, player);
            OnAnyObjectSpawned?.Invoke(this, kitchenObject);
        }
    }
}
