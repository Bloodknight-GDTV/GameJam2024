using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * This class is used to read the input from the player and send it to the PlayerController
 * It is a ScriptableObject so that it can be used as a singleton and be accessed from any class
 * It implements the IGameplayActions and IUIActions interfaces to read the input from the player
 * Interfaces are implemented by the generated class created by the input system.
 */

/**
 * Process for adding new input:
 * 1. Add the new input in the input actions asset
 * 2. Add the new input in the IGameplayActions or IUIActions interface **This step is often automated**
 * -----------------------------------------------------------------------------------------------
 * 3. Implement the new input in this class
 * -----------------------------------------------------------------------------------------------
 * 4. Subscribe to the new input in the class that needs it
 * 5. Implement the new input in the class that needs it
 * -----------------------------------------------------------------------------------------------
 * 6. Test the new input
 * 
 */

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
    // GameInput is the class generated by the input system.
    // It contains all the actions defined in the input actions asset (GameInputActions, UIActions, etc.)
    private GameInput gameInput;

    //***** list of events that can be subscribed to
    #region Event List
    
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action JumpCanceledEvent;
    public event Action PauseMenuEvent;
    public event Action ResumeEvent;
    public event Action SprintEvent;
    public event Action SprintCanceledEvent;
    public event Action<Vector2> CameraEvent;   
    
    
    #endregion
    
    
    // OnEnable is called when the scriptable object is created or enabled
    private void OnEnable()
    {
        if(gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Gameplay.SetCallbacks(this);
            gameInput.UI.SetCallbacks(this);
        }
        
        EnableGameplayInput();
    }
    
    
    // Pause menu with switching inputs from gameplay to UI and vice versa
    // Designed to be called from the GameManager
    // DesignChoice: Disable all inputs except one
    public void EnableGameplayInput()
    {
        gameInput.Gameplay.Enable();
        // Disable all other inputs
        gameInput.UI.Disable();
    }
    
    
    public void EnableUIInput()
    {
        gameInput.UI.Enable();
        // Disable all other inputs
        gameInput.Gameplay.Disable();
    }

    
    
    
    //***** IGameplayActions
    #region IGameplayActions
    
    public virtual void OnMove(InputAction.CallbackContext context)
    {
        // context.phase returns the phase of the input (started, performed, canceled)
        // context.ReadValue<Vector2>() returns the value of the input
        //Debug.Log($"Phase: {context.phase} OnMove: {context.ReadValue<Vector2>()}");
        
        
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        CameraEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log($"Phase: {context.phase} OnJump");
        
        // Jump event is triggered when the jump button is pressed there is no value to read
        if (context.phase == InputActionPhase.Started)
        {
            JumpEvent?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            JumpCanceledEvent?.Invoke();
        }
    }

   

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            SprintEvent?.Invoke();
            Debug.Log($"Phase: {context.phase} OnSprint");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            SprintCanceledEvent?.Invoke();
            Debug.Log($"Phase: {context.phase} OnSprint");
        }
    }
    
    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            PauseMenuEvent?.Invoke();
            EnableUIInput();
        }
        //Debug.Log($"Phase: {context.phase} OnPause");
    }
    

    #endregion

    //***** IUIActions
    #region IUIActions
    
    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ResumeEvent?.Invoke();
            EnableGameplayInput();
        }
        //Debug.Log($"Phase: {context.phase} OnResume");
    }
    
    #endregion

    //***** Auto created but unused
    #region Unused Stuff
    /* UI Actions */
    /*
    public virtual void OnPointerPosition(InputAction.CallbackContext context)
    {
    }

    public virtual void OnClick(InputAction.CallbackContext context)
    {
    }

    public virtual void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public virtual void OnCancel(InputAction.CallbackContext context)
    {
    }
    */
    #endregion
    
}
