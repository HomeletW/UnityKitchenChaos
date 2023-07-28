using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : KitchenCounter
{
    public static event EventHandler OnAnyItemTrashed;

    public static void ResetStaticData()
    {
        OnAnyItemTrashed = null;
    }

    protected override void InteractAction(Player player)
    {
        if (player.HoldingObject)
        {
            player.GetCurrentKitchenObject().DestroySelf();
            OnAnyItemTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
