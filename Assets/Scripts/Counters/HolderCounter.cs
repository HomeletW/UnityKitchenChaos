using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Base class of counters that are capable of holding some kitchen object
/// </summary>
public class HolderCounter : KitchenCounter, IKitchenObjectHolder
{
    public static event EventHandler<KitchenObject> OnAnyItemPlaced;

    public static void ResetStaticData()
    {
        OnAnyItemPlaced = null;
    }

    protected KitchenObject currentKitchenObject;

    public bool HoldingObject => (this as IKitchenObjectHolder).HoldingKitchenObject;

    public KitchenObject GetCurrentKitchenObject()
    {
        return currentKitchenObject;
    }

    public void SetCurrentKitchenObject(KitchenObject newObject)
    {
        if (newObject == null)
        {
            // clear the current counter
            currentKitchenObject = null;
        }
        else if (currentKitchenObject != null)
        {
            Debug.LogError("Counter cannot hold multiple KitchenObject");
        }
        else
        {
            currentKitchenObject = newObject;
            OnAnyItemPlaced?.Invoke(this, newObject);
        }
    }

    public Transform GetReferenceTransform()
    {
        return spawnPoint;
    }
}

