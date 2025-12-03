using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    private PlayerInputActions PlayerInputActions;
    
    private void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Player.Enable();

        PlayerInputActions.Player.Interact.performed += Interact_performed;
        PlayerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 InputVector = PlayerInputActions.Player.Move.ReadValue<Vector2>();

        InputVector = InputVector.normalized;
        return InputVector;
    }
}
