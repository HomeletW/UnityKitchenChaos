using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : ClearCounter, IProgressTracked
{
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burnt
    }

    public event EventHandler<StoveStateUpdateArg> OnStoveStateUpdate;
    public event EventHandler<IProgressTracked.ProgressChangedArg> OnProgressChanged;
    public class StoveStateUpdateArg : EventArgs, IProgressTracked.ProgressChangedArg
    {
        public State state;
        public bool isWarningRecipe;

        public bool IsBarActive()
        {
            return state == State.Frying;
        }

        public bool IsStoveActive()
        {
            return state == State.Frying;
        }

        public bool IsWarning()
        {
            return isWarningRecipe;
        }
    }

    public float GetNormalizedProgress()
    {
        Debug.Assert(Frying);
        return FryingProgressNormalized;
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipes;

    private FryingRecipeSO activeFryingRecipe;
    private float fryTimer = 0;
    private State currentState = State.Idle;

    private bool Frying => activeFryingRecipe != null;
    private bool DoneFrying => fryTimer >= activeFryingRecipe.fryingTime;
    private float FryingProgressNormalized => fryTimer / activeFryingRecipe.fryingTime;
    private bool IsWarningRecipe => Frying && activeFryingRecipe.shouldWarning;

    private void Start()
    {
        OnCounterItemChange += StoveCounter_OnCounterItemChange;
        OnStoveStateUpdate += (obj, arg) => OnProgressChanged.Invoke(obj, arg);
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.GameStateChangedArg e)
    {
        if (e.newState == GameManager.State.GAME_START_COUNTDOWN)
        {
            ResetFrying();
        }
    }

    private void StoveCounter_OnCounterItemChange(object sender, KitchenObject e)
    {
        if (HoldingObject)
        {
            // Player placed a new object on the stove, init frying
            FryingRecipeSO recipe = GetFryingRecipe(currentKitchenObject.GetKitchenObjectSO());
            Debug.Assert(recipe != null);
            InitalizeFrying(recipe);
        }
        else
        {
            // Player removed an object
            ResetFrying();
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Frying:
                HandleFrying();
                break;
            case State.Fried:
                HandleFried();
                break;
            case State.Burnt:
                HandleBurnt();
                break;
        }
    }

    protected override bool FilterAllowedObject(KitchenObject kitchenObject)
    {
        // only allowed fryable object to be placed
        return GetFryingRecipe(kitchenObject.GetKitchenObjectSO()) != null;
    }

    #region Frying Recipe Actions

    private FryingRecipeSO GetFryingRecipe(KitchenObjectSO from)
    {
        foreach (FryingRecipeSO recipe in fryingRecipes)
        {
            if (recipe.from == from)
            {
                return recipe;
            }
        }
        return null;
    }

    #endregion

    #region Frying Actions

    private void InitalizeFrying(FryingRecipeSO recipe)
    {
        activeFryingRecipe = recipe;
        fryTimer = 0;
        SetState(State.Frying);
    }

    private void ResetFrying()
    {
        activeFryingRecipe = null;
        fryTimer = 0;
        SetState(State.Idle);
    }

    private void HandleIdle()
    {
        // does nothing
    }

    private void HandleFrying()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            return;
        }

        // Advance the fry timer
        fryTimer += Time.deltaTime;
        if (DoneFrying)
        {
            // after timer runs out, go to fried
            currentKitchenObject.DestroySelf();
            KitchenObject.Spawn(activeFryingRecipe.to, this);
            SetState(State.Fried);
        }
    }

    private void HandleFried()
    {
        // checks for the next recipe
        FryingRecipeSO recipe = GetFryingRecipe(currentKitchenObject.GetKitchenObjectSO());
        if (recipe)
        {
            // Continue frying with the new recipe
            InitalizeFrying(recipe);
        }
        else
        {
            // No recipe for next step, go to burnt
            SetState(State.Burnt);
        }
    }

    private void HandleBurnt()
    {
        // does nothing
    }

    private void SetState(State newState)
    {
        currentState = newState;
        OnStoveStateUpdate?.Invoke(this, new StoveStateUpdateArg
        {
            state = currentState,
            isWarningRecipe = IsWarningRecipe
        });
    }

    #endregion
}
