using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }

    private IKitchenObjectHolder currentHolder;

    #region Holder Logic

    public void SetHolder(IKitchenObjectHolder newHolder)
    {
        // current holder might be null, indicating no one was holding it
        currentHolder?.SetCurrentKitchenObject(null);

        // new holder might also be null, indicating the previous owner wants to discards it
        newHolder?.SetCurrentKitchenObject(this);

        // correctly transfer the ownership
        currentHolder = newHolder;

        if (newHolder != null)
        {
            // sets up the transform
            transform.parent = newHolder.GetReferenceTransform();
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.parent = null;
        }
    }

    public IKitchenObjectHolder GetHolder()
    {
        return currentHolder;
    }

    #endregion

    public bool TryGetObject<T>(out T output) where T : KitchenObject
    {
        if (this is T)
        {
            output = this as T;
            return true;
        }
        else
        {
            output = null;
            return false;
        }
    }

    public bool AddToPlate(PlateKitchenObject plate)
    {
        if (plate.TryAddIngredient(kitchenObjectSO))
        {
            DestroySelf();
            return true;
        }
        else
        {
            Debug.LogFormat("{0} cannot be put on plate {1}", kitchenObjectSO.objectName, plate);
            return false;
        }
    }

    public void DestroySelf()
    {
        SetHolder(null);
        Destroy(gameObject);
        // Debug.Log("destroyed " + kitchenObjectSO.objectName);
    }

    public static KitchenObject Spawn(KitchenObjectSO koSO, IKitchenObjectHolder holder)
    {
        GameObject spawned = Instantiate(koSO.prefab);
        KitchenObject newKitchenObj = spawned.GetComponent<KitchenObject>();
        newKitchenObj.SetHolder(holder);
        // Debug.Log("spawned " + koSO.objectName);

        return newKitchenObj;
    }
}
