using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string GAME_INPUT_REBIND_OVERRIDE = "GameInputRebindOverride";

    public static GameInput Instance { get; private set; }

    public enum Binding
    {
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT,
        SPEED_UP,
        INTERACT,
        INTERACT_ALT,
        PAUSE,
        GAMEPAD_SPEED_UP,
        GAMEPAD_INTERACT,
        GAMEPAD_INTERACT_ALT,
        GAMEPAD_PAUSE,
    }

    private PlayerInputActions inputActions;

    public event EventHandler OnPlayerInteract;
    public event EventHandler OnPlayerInteractAlt;
    public event EventHandler OnPlayerPause;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Cannot have multiple instance of GameInput");
        }
        Instance = this;

        inputActions = new();
        if (PlayerPrefs.HasKey(GAME_INPUT_REBIND_OVERRIDE))
        {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(GAME_INPUT_REBIND_OVERRIDE));
        }

        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += (_) => OnPlayerInteract?.Invoke(this, EventArgs.Empty);
        inputActions.Player.InteractAlternate.performed += (_) => OnPlayerInteractAlt?.Invoke(this, EventArgs.Empty);
        inputActions.Player.Pause.performed += (_) => OnPlayerPause?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }

    public Vector2 GetMovementVector()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

    public bool IsSpeedingUp()
    {
        return inputActions.Player.SpeedUp.IsPressed();
    }

    public string GetKeyboardBindingText(Binding binding)
    {
        return binding switch
        {
            Binding.MOVE_UP => inputActions.Player.Move.bindings[1].ToDisplayString(),
            Binding.MOVE_DOWN => inputActions.Player.Move.bindings[2].ToDisplayString(),
            Binding.MOVE_LEFT => inputActions.Player.Move.bindings[3].ToDisplayString(),
            Binding.MOVE_RIGHT => inputActions.Player.Move.bindings[4].ToDisplayString(),
            Binding.SPEED_UP => inputActions.Player.SpeedUp.bindings[0].ToDisplayString(),
            Binding.INTERACT => inputActions.Player.Interact.bindings[0].ToDisplayString(),
            Binding.INTERACT_ALT => inputActions.Player.InteractAlternate.bindings[0].ToDisplayString(),
            Binding.PAUSE => inputActions.Player.Pause.bindings[0].ToDisplayString(),
            Binding.GAMEPAD_SPEED_UP => inputActions.Player.SpeedUp.bindings[1].ToDisplayString(),
            Binding.GAMEPAD_INTERACT => inputActions.Player.Interact.bindings[1].ToDisplayString(),
            Binding.GAMEPAD_INTERACT_ALT => inputActions.Player.InteractAlternate.bindings[1].ToDisplayString(),
            Binding.GAMEPAD_PAUSE => inputActions.Player.Pause.bindings[1].ToDisplayString(),
            _ => null,
        };
    }

    public void InteractiveRebind(Binding binding, Action rebindCompleteCallback = null)
    {
        switch (binding)
        {
            case Binding.MOVE_UP:
                PerformInteractiveRebind(inputActions.Player.Move, 1, rebindCompleteCallback);
                break;
            case Binding.MOVE_DOWN:
                PerformInteractiveRebind(inputActions.Player.Move, 2, rebindCompleteCallback);
                break;
            case Binding.MOVE_LEFT:
                PerformInteractiveRebind(inputActions.Player.Move, 3, rebindCompleteCallback);
                break;
            case Binding.MOVE_RIGHT:
                PerformInteractiveRebind(inputActions.Player.Move, 4, rebindCompleteCallback);
                break;
            case Binding.SPEED_UP:
                PerformInteractiveRebind(inputActions.Player.SpeedUp, 0, rebindCompleteCallback);
                break;
            case Binding.INTERACT:
                PerformInteractiveRebind(inputActions.Player.Interact, 0, rebindCompleteCallback);
                break;
            case Binding.INTERACT_ALT:
                PerformInteractiveRebind(inputActions.Player.InteractAlternate, 0, rebindCompleteCallback);
                break;
            case Binding.PAUSE:
                PerformInteractiveRebind(inputActions.Player.Pause, 0, rebindCompleteCallback);
                break;
        }
    }

    private void PerformInteractiveRebind(InputAction action, int index, Action rebindCompleteCallback)
    {
        inputActions.Player.Disable();
        action.PerformInteractiveRebinding(index).OnComplete(op =>
        {
            PlayerPrefs.SetString(GAME_INPUT_REBIND_OVERRIDE, inputActions.SaveBindingOverridesAsJson());
            inputActions.Player.Enable();
            rebindCompleteCallback?.Invoke();
            op.Dispose();
        }).Start();
    }
}