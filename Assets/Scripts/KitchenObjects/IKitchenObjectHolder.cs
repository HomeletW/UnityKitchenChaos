using System;
using UnityEngine;

public interface IKitchenObjectHolder
{
    /// <summary>
    /// Return the currently holding object, return null if not holding any
    /// </summary>
    /// <returns></returns>
    public KitchenObject GetCurrentKitchenObject();

    /// <summary>
    /// Set the currently holding object to newObject.
    /// </summary>
    public void SetCurrentKitchenObject(KitchenObject newObject);

    /// <summary>
    /// Return the reference transform that should be set as the parent
    /// </summary>
    public Transform GetReferenceTransform();

    /// <summary>
    /// True if currently holding an object
    /// </summary>
    public bool HoldingKitchenObject => GetCurrentKitchenObject() != null;
}
