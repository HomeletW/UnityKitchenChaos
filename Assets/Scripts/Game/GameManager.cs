using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler<GameStateChangedArg> OnGameStateChanged;
    public class GameStateChangedArg : EventArgs
    {
        public State lastState;
        public State newState;
    }

    public enum State
    {
        GAME_START_WAITING,
        GAME_START_COUNTDOWN,
        GAME_RUNNING,
        GAME_PAUSING,
        GAME_END,
    }

    [SerializeField] private float gameStartCountdownTime = 4f;
    [SerializeField] private float gameTime = 60f;

    private State currentState, previousState;
    public bool IsPlaying => currentState == State.GAME_RUNNING;

    private bool showingTutorial = false;

    private bool paused = false;
    private Action pauseResolvedAction = null;

    private float gameStartCountdown = 0f;
    public bool GameStartCountdownEnded => gameStartCountdown <= 0;
    public int GameStartCountdownFloored => (int)MathF.Floor(gameStartCountdown);

    private float gameRunningCountdown = 0f;
    public bool GameRunningCountdownEnded => gameRunningCountdown <= 0;
    public int GameRunningCountdownFloored => (int)MathF.Floor(gameRunningCountdown);

    private int plateServed;
    public int PlateServed => plateServed;

    private int ingredientSpawned;
    private int ingredientUsed;
    public float IngredientUtilityPercent => ingredientSpawned == 0 ? 0f : MathF.Ceiling((float)ingredientUsed / ingredientSpawned * 100);

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Cannot have multiple instance of GameManager");
        }
        Instance = this;
        previousState = State.GAME_START_WAITING;
        currentState = State.GAME_START_WAITING;
    }

    private void Start()
    {
        ContainerCounter.OnAnyObjectSpawned += (_, _) =>
        {
            ++ingredientSpawned;
        };
        DeliveryManager.Instance.OnOrderDelivered += (_, recipe) =>
        {
            ++plateServed;
            ingredientUsed += recipe.ingredients.Count;
        };
        GameInput.Instance.OnPlayerPause += (_, _) => RequestPause();
        StartCoroutine(RunStateMachine());
    }

    private void RequestPause()
    {
        paused = true;
    }

    public void ExitPause(Action pauseResolvedAction = null)
    {
        paused = false;
        this.pauseResolvedAction = pauseResolvedAction;
    }

    public void ExitTutorial()
    {
        showingTutorial = false;
    }

    #region State Machine

    private IEnumerator RunStateMachine()
    {
        yield return null;  // wait for one frame
        SetState(State.GAME_START_WAITING);
        while (true)
        {
            switch (currentState)
            {
                case State.GAME_START_WAITING:
                    yield return HandleGameStartWaiting();
                    break;
                case State.GAME_START_COUNTDOWN:
                    yield return HandleGameStartCountdown();
                    break;
                case State.GAME_RUNNING:
                    yield return HandleGameRunning();
                    break;
                case State.GAME_PAUSING:
                    yield return HandleGamePausing();
                    break;
                case State.GAME_END:
                    yield return HandleGameEnd();
                    break;
            }
        }
    }

    private IEnumerator HandleGameStartWaiting()
    {
        showingTutorial = true;
        yield return new WaitUntil(() => !showingTutorial);
        gameStartCountdown = gameStartCountdownTime;
        SetState(State.GAME_START_COUNTDOWN);
    }

    private IEnumerator HandleGameStartCountdown()
    {
        while (!GameStartCountdownEnded)
        {
            if (paused)
            {
                SetState(State.GAME_PAUSING);
                yield break;
            }
            gameStartCountdown -= Time.deltaTime;
            yield return null;   // wait for end of frame
        }
        ClearGameStat();
        gameRunningCountdown = gameTime;
        SetState(State.GAME_RUNNING);
    }

    private IEnumerator HandleGameRunning()
    {
        while (!GameRunningCountdownEnded)
        {
            if (paused)
            {
                SetState(State.GAME_PAUSING);
                yield break;
            }
            gameRunningCountdown -= Time.deltaTime;
            yield return null;   // wait for end of frame
        }
        SetState(State.GAME_END);
    }

    private IEnumerator HandleGamePausing()
    {
        Time.timeScale = 0;
        yield return new WaitUntil(() => !paused);
        Time.timeScale = 1;

        pauseResolvedAction?.Invoke();
        pauseResolvedAction = null;

        SetState(previousState);
    }

    private IEnumerator HandleGameEnd()
    {
        while (true)
        {
            yield return null;
        }
    }

    private void ClearGameStat()
    {
        plateServed = 0;
        ingredientSpawned = 0;
        ingredientUsed = 0;
    }

    private void SetState(State newState)
    {
        previousState = currentState;
        currentState = newState;
        OnGameStateChanged?.Invoke(this, new GameStateChangedArg
        {
            lastState = previousState,
            newState = currentState
        });
    }

    #endregion
}
