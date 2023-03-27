using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public KitchenObject KitchenObject { get; set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameInput gameInput;
    [SerializeField] LayerMask interactableLayerMask;
    [SerializeField] Transform kitchenObjectHoldPoint;

    bool isWalking;
    Vector3 lastInteractDirection;
    BaseCounter selectedCounter;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }


    void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }

        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, interactableLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        bool CapsuleCollision(Vector3 moveDirection)
            => Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        bool canMove = !CapsuleCollision(moveDirection);

        if (!canMove)
        {
            // Cannot move towards MoveDirection
            // Attempt only X Movement
            Vector3 moveDirectionX = new(moveDirection.x, 0, 0);
            canMove = moveDirection.x != 0 && !CapsuleCollision(moveDirectionX);

            if (canMove)
            {
                // Can only move in the X direction
                moveDirection = moveDirectionX.normalized;
            }
            else
            {
                // Cannot move towards X direction
                // Attempt only Z movement
                Vector3 moveDirectionZ = new(0, 0, moveDirection.z);
                canMove = moveDirection.z != 0 && !CapsuleCollision(moveDirectionZ);

                if (canMove)
                {
                    // Can only move in the Z direction
                    moveDirection = moveDirectionZ.normalized;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        isWalking = moveDirection != Vector3.zero;

        float rotationSpeed = 10f;

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
    }

    void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new() { selectedCounter = selectedCounter });
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public void ClearKitchenObject() => KitchenObject = null;
    public bool HasKitchenObject() => KitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => kitchenObjectHoldPoint;
}
