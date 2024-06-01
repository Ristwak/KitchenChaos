using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IKitchenObjectParent
{
    public static PlayerScript Instance{get; private set;}
    
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInputs gameInput;
    [SerializeField] private LayerMask counterLayermask;
    [SerializeField] private Transform KitchenObjectHoldPoint;



    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selecetdCounter;
    private KitchenObject kitchenObject;




    public float playerRadius = 1f;

    public float playerHeight = 1f;

    void Start()
    {
        gameInput.OnInteractAction += GameInputs_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInputs_OnInteractAlternateAction;
    }

    private void GameInputs_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if(!KitchenGameManager.Instance.isGamePlaying()) return;

        if (selecetdCounter != null)
        {
            selecetdCounter.InteractAlternate(this);
        }
    }


    private void GameInputs_OnInteractAction(object sender, System.EventArgs e)
    {
        if(!KitchenGameManager.Instance.isGamePlaying()) return;

        if (selecetdCounter != null)
        {
            selecetdCounter.Interact(this);
        }
    }

    void Awake()
    {
        if(Instance != null)
        {
            // Debug.LogError("There is more than one Player Instance");
        }
        Instance = this;
    }
    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayermask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has ClearCounter
                if (baseCounter != selecetdCounter)
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

    public bool is_Walking()
    {
        return isWalking;
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;

        // CapsuleCast is similar to Raycast except that it doesnt projects a line in forward direction instead it created a boundary around the object it is attached to
        // We can use this on a vehicle to make it run over characters
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            // this case will occur when we are press two direction key and we want to move(slide) the character in any particular direction when it is standing again any obstacle
            // Cannot move toward moveDir

            // Attempt moving in X-Direction
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                // can move only on X
                moveDir = moveDirX;
            }
            else
            {
                // cannot move only in X

                // Attempt Only on Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f ) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    // Can move only in Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        float rotationSpeed = 10f;
        // to make the rotation smooth Lerp and Slrp is used
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        // If you find Quaternion difficult to use and understand for rotation , you can use eulerAngles or LookAt or forward instead of QuaTernion 
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selecetdCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selecetdCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
