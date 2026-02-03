using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private PlayerInputActions PlayerInputActions;
    
    private void Awake()
    {
        Instance = this;

        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Player.Enable();

        PlayerInputActions.Player.Interact.performed += Interact_performed;
        PlayerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        PlayerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        PlayerInputActions.Player.Interact.performed -= Interact_performed;
        PlayerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        PlayerInputActions.Player.Pause.performed -= Pause_performed;

        PlayerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
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
