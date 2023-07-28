using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public static event EventHandler<KitchenObjectSO> OnAnyIngredientAdded;

    public static void ResetStaticData()
    {
        OnAnyIngredientAdded = null;
    }

    /// <summary>
    /// Called on a new ingredient added, always called after recipe changed is called
    /// </summary>
    public event EventHandler<PlateUpdatedArg> OnPlateUpdated;
    public class PlateUpdatedArg : EventArgs
    {
        public bool recipeChanged;
        public MenuRecipeSO currentRecipe;
        public List<KitchenObjectSO> currentIngredients;
        public KitchenObjectSO NewIngredient => currentIngredients[^1];  // the last index
    }

    private MenuRecipeSO currentRecipe;
    private List<KitchenObjectSO> currentIngredients;

    private void Awake()
    {
        currentIngredients = new List<KitchenObjectSO>();
    }

    public int GetNumIngredients()
    {
        return currentIngredients.Count;
    }

    public bool TryAddIngredient(KitchenObjectSO newIngredient)
    {
        if (newIngredient == GetKitchenObjectSO())
        {
            // cannot add a plate to a plate
            return false;
        }

        // Construct a new ingredients list with the new ingredient
        List<KitchenObjectSO> tempIngredients = new(currentIngredients)
        {
            newIngredient
        };

        // find the recipe that will make with the list
        MenuRecipeSO recipeCandidate = MenuManager.Instance.FindRecipeCandidate(tempIngredients);
        if (recipeCandidate)
        {
            bool recipeChanged = recipeCandidate != currentRecipe;
            if (recipeChanged)
            {
                currentRecipe = recipeCandidate;
            }
            currentIngredients = tempIngredients;

            OnPlateUpdated?.Invoke(this, new PlateUpdatedArg
            {
                recipeChanged = recipeChanged,
                currentRecipe = currentRecipe,
                currentIngredients = currentIngredients,
            });

            OnAnyIngredientAdded?.Invoke(this, newIngredient);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Try to deliver the current plate as a recipe, return false if cannot make
    // a recipe out of current ingredients
    public bool TryMakeMenuItem(out MenuRecipeSO deliveredRecipe)
    {
        if (currentRecipe != null && MenuManager.Instance.DoesMakeMenuItem(currentIngredients, currentRecipe))
        {
            deliveredRecipe = currentRecipe;
            return true;
        }
        else
        {
            deliveredRecipe = null;
            return false;
        }
    }

    public override string ToString()
    {
        string str = "Plate {";
        foreach (KitchenObjectSO ko in currentIngredients)
        {
            str += ko.objectName + ", ";
        }
        str += "}";
        return str;
    }
}
