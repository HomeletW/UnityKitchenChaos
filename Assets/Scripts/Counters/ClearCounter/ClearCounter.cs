using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A type of counter, capable of holding one kichen object
/// </summary>
public class ClearCounter : HolderCounter
{
    public event EventHandler<KitchenObject> OnCounterItemChange;

    protected override void InteractAction(Player player)
    {
        if (player.HoldingObject)
        {
            if (!this.HoldingObject)
            {
                if (!FilterAllowedObject(player.GetCurrentKitchenObject()))
                {
                    Debug.Log("Player cannot place " + player.GetCurrentKitchenObject().GetKitchenObjectSO().objectName + " on counter");
                }
                else
                {
                    // player is holding an object, and counter is not
                    // transfer the holder of the object to the counter
                    player.GetCurrentKitchenObject().SetHolder(this);

                    // after transfering, the current object should be null
                    OnCounterItemChange?.Invoke(this, null);
                }
            }
            else
            {
                bool counterHoldingPlate = currentKitchenObject.TryGetObject(out PlateKitchenObject counterPlate);
                bool playerHoldingPlate = player.GetCurrentKitchenObject().TryGetObject(out PlateKitchenObject playerPlate);

                if (counterHoldingPlate && !playerHoldingPlate)
                {
                    player.GetCurrentKitchenObject().AddToPlate(counterPlate);
                }
                else if (!counterHoldingPlate && playerHoldingPlate)
                {
                    if (this.currentKitchenObject.AddToPlate(playerPlate))
                    {
                        // after the current object been added to the plate
                        // invoke the on change handler
                        OnCounterItemChange?.Invoke(this, null);
                    }
                }
                else
                {
                    // in the case of both holding plates or both holding non-plates
                    // swap the holder of the two

                    KitchenObject tempObj = player.GetCurrentKitchenObject();
                    if (!FilterAllowedObject(player.GetCurrentKitchenObject()))
                    {
                        Debug.Log("Player cannot place " + player.GetCurrentKitchenObject().GetKitchenObjectSO().objectName + " on counter");
                    }
                    else
                    {
                        // let the player discards the current obj
                        // this is to prevent the breif moment that kitchenobject has two holder
                        tempObj.SetHolder(null);

                        // swap the ownership
                        this.currentKitchenObject.SetHolder(player);
                        tempObj.SetHolder(this);

                        // after transfering, the counter should owns the new item
                        OnCounterItemChange?.Invoke(this, tempObj);
                    }
                }
            }
        }
        else if (this.HoldingObject)
        {
            // player is not holding object, and counter is
            // transfer the holder of the object to the player
            this.currentKitchenObject.SetHolder(player);

            // after transfering, the current object should be null
            OnCounterItemChange?.Invoke(this, null);
        }
    }

    /// <summary>
    /// Returns true if the object is allowed to be placed, by default returns true to all
    /// </summary>
    protected virtual bool FilterAllowedObject(KitchenObject kitchenObject)
    {
        return true;
    }
}
