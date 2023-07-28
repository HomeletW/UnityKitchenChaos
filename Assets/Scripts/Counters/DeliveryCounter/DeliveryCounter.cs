using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : KitchenCounter
{
    protected override void InteractAction(Player player)
    {
        if (player.HoldingObject)
        {
            if (player.GetCurrentKitchenObject().TryGetObject(out PlateKitchenObject plate))
            {
                if (DeliveryManager.Instance.TryDeliverPlate(plate))
                {
                    plate.DestroySelf();
                }
            }
            else
            {
                Debug.Log("Cannot delivery without a plate");
            }
        }
    }
}
