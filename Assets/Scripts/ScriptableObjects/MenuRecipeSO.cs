using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MenuRecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> ingredients;
    public GameObject prefabVisual;
    public string recipeName;
}
