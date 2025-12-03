using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    [SerializeField] private float MoveSpeed = 7f; // Added MoveSpeed variable.
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask CounterLayerMask;
    [SerializeField] private Transform KitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 LastInteractDirection;
    private BaseCounter SelectedCounter;
    private KitchenObject KitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player Instance");
        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (SelectedCounter != null)
        {
            SelectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (SelectedCounter != null)
        {
            SelectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();
        Vector3 MoveDirection = new Vector3(InputVector.x, 0, InputVector.y);

        if (MoveDirection != Vector3.zero)
        {
            LastInteractDirection = MoveDirection;
        }

        float InteractDistance = 2f;

        if (Physics.Raycast(transform.position, LastInteractDirection, out RaycastHit raycastHit, InteractDistance, CounterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter BaseCounter))
            {
                if (BaseCounter != SelectedCounter)
                {
                    SetSelectedCounter(BaseCounter);
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

    private void HandleMovement()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();
        Vector3 MoveDirection = new Vector3(InputVector.x, 0, InputVector.y);

        float MoveDistance = MoveSpeed * Time.deltaTime;
        float PlayerRadius = .7f;
        float PlayerHeight = 2f;
        bool CanMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, MoveDirection, MoveDistance);

        if (!CanMove)
        {
            //Cannot move towards movedirection

            //Attempt only X Movement
            Vector3 MoveDirectionX = new Vector3(MoveDirection.x, 0, 0).normalized;
            CanMove = MoveDirection.x !=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, MoveDirectionX, MoveDistance);
            if (CanMove)
            {
                //Can only move in x direction
                MoveDirection = MoveDirectionX;
            }
            else
            {
                //cannot move on x direction
                Vector3 MoveDirectionZ = new Vector3(0, 0, MoveDirection.z).normalized;
                CanMove = MoveDirection.z !=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, MoveDirectionZ, MoveDistance);
                if (CanMove)
                {
                    //can only move on z direction
                    MoveDirection = MoveDirectionZ;
                }
                else
                {
                    //Cannot move in any direction
                }

            }

        }

        if (CanMove)
        {
            transform.position += MoveDirection * MoveSpeed * Time.deltaTime;
        }
        isWalking = MoveDirection != Vector3.zero;

        float RotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, MoveDirection, Time.deltaTime * RotateSpeed);
    }
    private void SetSelectedCounter(BaseCounter SelectedCounter)
    {
        this.SelectedCounter = SelectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = SelectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject KitchenObject)
    {
        this.KitchenObject = KitchenObject;
    }
    public KitchenObject GetKitchenObject()
    {
        return KitchenObject;
    }
    public void ClearKitchenObject()
    {
        KitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return KitchenObject != null;
    }
}
