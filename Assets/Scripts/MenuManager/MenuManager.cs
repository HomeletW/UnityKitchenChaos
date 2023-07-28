using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    // The order of item should be sorted in increasing order of amount of ingredients
    [SerializeField] private MenuSO menu;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Cannot have mutiple instance of MenuManager");
        }
        Instance = this;
    }

    /// <summary>
    /// Preq: ingredients does make recipe, i.e. recipe was returned by FindRecipeCandidate
    /// </summary>
    public bool DoesMakeMenuItem(List<KitchenObjectSO> ingredients, MenuRecipeSO recipe)
    {
        return CanMake(ingredients, recipe);
    }

    public MenuRecipeSO FindRecipeCandidate(List<KitchenObjectSO> ingredients)
    {
        foreach (MenuRecipeSO recipe in menu.menuRecipes)
        {
            if (WillMake(ingredients, recipe))
            {
                return recipe;
            }
        }
        return null;
    }

    // The ingredients will make the recipe when current ingredients are subset of recipe ingredients
    private bool WillMake(List<KitchenObjectSO> ingredients, MenuRecipeSO recipe)
    {
        ISet<int> ingrSet = new HashSet<int>();
        foreach (KitchenObjectSO plateIngr in ingredients)
        {
            int ingrIndex = FindNotInSet(recipe.ingredients, plateIngr, ingrSet);
            if (ingrIndex < 0)
            {
                return false;
            }
            ingrSet.Add(ingrIndex);
        }
        return true;
    }

    // The ingredients can make the recipe when the recipe ingredients are subset of current ingredients
    private bool CanMake(List<KitchenObjectSO> ingredients, MenuRecipeSO recipe)
    {
        ISet<int> ingrSet = new HashSet<int>();
        foreach (KitchenObjectSO recipeIngr in recipe.ingredients)
        {
            int ingrIndex = FindNotInSet(ingredients, recipeIngr, ingrSet);
            if (ingrIndex < 0)
            {
                return false;
            }
            ingrSet.Add(ingrIndex);
        }
        return true;
    }

    private int FindNotInSet(List<KitchenObjectSO> soList, KitchenObjectSO so, ISet<int> exclusionSet)
    {
        for (int index = 0; index < soList.Count; ++index)
        {
            if (exclusionSet.Contains(index))
            {
                continue;
            }
            else if (soList[index] != so)
            {
                continue;
            }
            return index;
        }
        return -1;
    }
}
