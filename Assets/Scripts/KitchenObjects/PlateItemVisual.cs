using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateItemVisual : MonoBehaviour
{
    private PlateKitchenObject plate;
    private GameObject plateItem;

    private void Awake()
    {
        plate = GetComponentInParent<PlateKitchenObject>();
        // called here because Start is not called on Instansiate
        plate.OnPlateUpdated += Plate_OnPlateUpdated;
    }

    private void Start()
    {
    }

    private void Plate_OnPlateUpdated(object sender, PlateKitchenObject.PlateUpdatedArg e)
    {
        if (plateItem == null || e.recipeChanged)
        {
            // perform of complete sync of the visual
            if (plateItem != null)
            {
                Destroy(plateItem);
            }

            // instantiate the prefab under current object's transform
            plateItem = Instantiate(e.currentRecipe.prefabVisual, transform);
            plateItem.transform.localPosition = Vector3.zero;

            // disable all children at first
            foreach (Transform plateItemChild in plateItem.transform)
            {
                plateItemChild.gameObject.SetActive(false);
            }

            // find the corrospoinding visual item and set it to active
            foreach (KitchenObjectSO ingredient in e.currentIngredients)
            {
                SyncIngredientVisual(e.currentRecipe, ingredient);
            }
        }
        else
        {
            // handles the new ingredient
            SyncIngredientVisual(e.currentRecipe, e.NewIngredient);
        }
    }

    private void SyncIngredientVisual(MenuRecipeSO recipe, KitchenObjectSO ingredient)
    {
        for (int index = 0; index < recipe.ingredients.Count; ++index)
        {
            // new ingredient should be matched to an previously not active item
            if (recipe.ingredients[index] == ingredient && !plateItem.transform.GetChild(index).gameObject.activeSelf)
            {
                plateItem.transform.GetChild(index).gameObject.SetActive(true);
                return;
            }
        }
        Debug.LogErrorFormat("Cannot find visual for {0} in recipe {1}", ingredient.objectName, recipe.recipeName);
    }
}
