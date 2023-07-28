using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectHolder
{
    // Assumes a single player
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangeEventArg> OnSelectedCounterChange;
    public class OnSelectedCounterChangeEventArg : EventArgs
    {
        public KitchenCounter selected;
    }

    public event EventHandler<KitchenObject> OnPlayerPickedUp;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float speedUpScale = 2.0f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = .7f;
    [SerializeField] private float playerInteractDistance = 2f;
    [SerializeField] private float diagnalMovementThreshold = 0.5f;
    [SerializeField] private LayerMask playerInteractLayerMask;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform objectPickupPoint;

    private KitchenObject currentKitchenObject = null;
    private bool isWalking = false;
    private bool isSprinting = false;
    private KitchenCounter selectedCounter = null;

    public bool IsWalking => isWalking;
    public bool IsSprinting => isSprinting;
    public bool HoldingObject => (this as IKitchenObjectHolder).HoldingKitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Cannot have multiple instance of player");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnPlayerInteract += InteractionAction;
        gameInput.OnPlayerInteractAlt += InteractAltAction;
    }

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVector();

        isWalking = inputVector != Vector2.zero;
        if (isWalking)
        {
            Vector3 moveDirection = new(inputVector.x, 0f, inputVector.y);
            HandleMovement(moveDirection);
        }

        HandleInteraction();
    }

    #region Movement Handler

    private void HandleMovement(Vector3 moveDirection)
    {
        float moveDistance = moveSpeed * Time.deltaTime;

        isSprinting = gameInput.IsSpeedingUp();
        if (isSprinting)
        {
            moveDistance *= speedUpScale;
        }

        Vector3 actualMoveTransform = GetNormalizedMovementTransform(moveDirection, moveDistance);
        if (actualMoveTransform != Vector3.zero)
        {
            // move player
            transform.position += actualMoveTransform;
        }

        // rotate player to face the direction
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    private Vector3 GetNormalizedMovementTransform(Vector3 moveDirection, float moveDistance)
    {
        Vector3 PlayerBottom = transform.position;
        Vector3 PlayerTop = transform.position + playerHeight * Vector3.up;
        bool CanMoveTowards(Vector3 dir, float dist) =>
            !Physics.CapsuleCast(PlayerBottom, PlayerTop, playerRadius, dir, dist);

        if (CanMoveTowards(moveDirection, moveDistance))
        {
            return moveDirection.normalized * moveDistance;
        }

        // otherwise, try the x component
        Vector3 alternativeDirection = new(moveDirection.x, 0, 0);
        if (IsMovementAboveThreshold(moveDirection.x) && CanMoveTowards(alternativeDirection, moveDistance))
        {
            return alternativeDirection.normalized * moveDistance;
        }

        // finally, try the z component
        alternativeDirection.x = 0;
        alternativeDirection.z = moveDirection.z;
        if (IsMovementAboveThreshold(moveDirection.z) && CanMoveTowards(alternativeDirection, moveDistance))
        {
            return alternativeDirection.normalized * moveDistance;
        }

        // if none worked, don't move
        return Vector3.zero;
    }

    private bool IsMovementAboveThreshold(float movement)
    {
        return movement < -diagnalMovementThreshold || movement > diagnalMovementThreshold;
    }

    #endregion

    #region Interaction Handler

    private void HandleInteraction()
    {
        // get what's in front of player
        bool interactHit = Physics.Raycast(transform.position, transform.forward,
            out RaycastHit interactObject,
            playerInteractDistance, playerInteractLayerMask);
        if (interactHit && interactObject.transform.TryGetComponent(out KitchenCounter counter))
        {
            SetSelectedCounter(counter);
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void InteractionAction(object sender, EventArgs args)
    {
        if (selectedCounter != null && GameManager.Instance.IsPlaying)
        {
            selectedCounter.Interact(this);
        }
    }

    private void InteractAltAction(object sender, EventArgs args)
    {
        if (selectedCounter != null && GameManager.Instance.IsPlaying)
        {
            selectedCounter.InteractAlt(this);
        }
    }

    private void SetSelectedCounter(KitchenCounter newCounter)
    {
        selectedCounter = newCounter;
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeEventArg { selected = newCounter });
    }

    #endregion

    #region IKitchenObjectHolder Impl

    public KitchenObject GetCurrentKitchenObject()
    {
        return currentKitchenObject;
    }

    public void SetCurrentKitchenObject(KitchenObject newObject)
    {
        if (newObject == null)
        {
            currentKitchenObject = null;
        }
        else if (currentKitchenObject != null)
        {
            Debug.LogError("Player cannot hold multiple KitchenObject");
        }
        else
        {
            currentKitchenObject = newObject;
            OnPlayerPickedUp?.Invoke(this, newObject);
        }
    }

    public Transform GetReferenceTransform()
    {
        return objectPickupPoint;
    }

    #endregion
}
