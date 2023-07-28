using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A type of counter, capable of holding, and processing the item when alt interacted with
/// </summary>
public class CuttingCounter : ClearCounter, IProgressTracked
{
    public static event EventHandler OnAnyCut;

    public static new void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<CutProgressUpdatedArg> OnCut;
    public event EventHandler<IProgressTracked.ProgressChangedArg> OnProgressChanged;
    public class CutProgressUpdatedArg : EventArgs, IProgressTracked.ProgressChangedArg
    {
        public bool active;

        public bool IsBarActive()
        {
            return active;
        }

        public bool IsWarning()
        {
            return false;
        }
    }

    public float GetNormalizedProgress()
    {
        Debug.Assert(Cutting);
        return CuttingProgressNormalized;
    }

    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

    private CuttingRecipeSO activeCuttingRecipe;
    private int cuttingProgress;

    private bool Cutting => activeCuttingRecipe != null;
    private bool DoneCutting => cuttingProgress >= activeCuttingRecipe.cutCount;
    private float CuttingProgressNormalized => (float)cuttingProgress / activeCuttingRecipe.cutCount;

    private void Start()
    {
        // each time the counter changes item, clear the cutting progress
        OnCounterItemChange += (_, _) => ResetCutting();

        // chain the onCut event with the onProgressChanged event and onAnyCut
        OnCut += (obj, arg) => OnProgressChanged.Invoke(obj, arg);
        OnCut += (obj, _) => OnAnyCut?.Invoke(obj, EventArgs.Empty);
    }

    protected override void InteractAltAction(Player player)
    {
        if (Cutting)
        {
            Cut();
        }
        else if (HoldingObject)
        {
            // destroy the current object, and spawns the cutted one
            CuttingRecipeSO recipe = GetCuttingRecipe(currentKitchenObject.GetKitchenObjectSO());
            if (recipe)
            {
                InitalizeCutting(recipe);
                Cut();
            }
            else
            {
                Debug.Log(currentKitchenObject.GetKitchenObjectSO().objectName + " cannot be cut");
            }
        }
    }

    protected override bool FilterAllowedObject(KitchenObject kitchenObject)
    {
        return kitchenObject.GetKitchenObjectSO().objectName != "Plate";
    }

    #region Recipe Actions

    private CuttingRecipeSO GetCuttingRecipe(KitchenObjectSO from)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipes)
        {
            if (recipe.from == from)
            {
                return recipe;
            }
        }
        return null;
    }

    #endregion

    #region Cutting Actions

    private void InitalizeCutting(CuttingRecipeSO recipe)
    {
        if (Cutting)
        {
            Debug.LogError("Cannot have multiple active cutting recipe");
        }
        else
        {
            activeCuttingRecipe = recipe;
            SetProgress(0);
        }
    }

    private void ResetCutting()
    {
        activeCuttingRecipe = null;
        SetProgress(0);
    }

    private void Cut()
    {
        if (Cutting)
        {
            SetProgress(cuttingProgress + 1);

            // Checks for done cutting
            if (DoneCutting)
            {
                currentKitchenObject.DestroySelf();
                KitchenObject.Spawn(activeCuttingRecipe.to, this);
                ResetCutting();
            }
        }
        else
        {
            Debug.LogError("Cannot cut when with no active cutting recipe");
        }
    }

    private void SetProgress(int newProgress)
    {
        cuttingProgress = newProgress;
        OnCut?.Invoke(this, new CutProgressUpdatedArg
        {
            active = cuttingProgress != 0
        });
    }

    #endregion
}
