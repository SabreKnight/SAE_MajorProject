using UnityEngine;
using UnityEngine.InputSystem;

// input manager is a controller in controller, model, view networking framework
public class InputManager_Cont : MonoBehaviour
{
    public InputActionAsset InputAction;

# region Movement Inputs
    private InputAction moveAction;
    [Header ("Movement")]
    [SerializeField] private float moveSpeed = 0.01f; // base movement speed for the player
    [SerializeField] private GameObject PlayerObject;   // reference to the player object
    [SerializeField] private Vector2 moveValue; // vector  movement value, shows direction player is moving (normalised)

    private InputAction shiftAction;    // checks if shift / sprint is being pressed
    [SerializeField] private float sprintSpeed = 2f;    // a scale factor for the player to increase while sprinting based on move speed

# endregion

# region InteractionInputs

    private InputAction leftMouse;  // left mouse click aciton
    private InputAction rightMouse; // right mouse click action
    private float LeftClickCount = 0;   // time the left click has been held for
    private float RightClickCount = 0;  // time the right click has been held for

    private InputAction InteractAction; // interact input action (E Key)
    private InputAction EscapeAction;   // Escape Input action (ESC Key)

# endregion

# region PointingDirection

    private InputAction lookAction;
    [SerializeField] private Vector2 lookDirection;

# endregion
    
    void OnEnable() // when the input script is enabled
    {
        //InputAction.FindActionMap("Player");

        //this gets a reference to each of the actions in the player input map
        moveAction = InputSystem.actions.FindAction("Move");
        leftMouse = InputSystem.actions.FindAction("Primary");
        rightMouse = InputSystem.actions.FindAction("Secondary");
        shiftAction = InputSystem.actions.FindAction("Sprint");
        InteractAction = InputSystem.actions.FindAction("Interact");
        EscapeAction = InputSystem.actions.FindAction("Escape");
        lookAction = InputSystem.actions.FindAction("Look");
    }

    private void Update()
    {
        CheckMisc();
        CheckLeftMouseInput();
        CheckRightMouseInput();
    }

    private void CheckMisc()    // checks any miscellanious inputs short enough for just 1 functions
    {
        //Checks Movement values
        moveValue = moveAction.ReadValue<Vector2>();    // checks movement values for the player
        MovePlayer(moveValue);  // calls movement script

        lookDirection = lookAction.ReadValue<Vector2>(); // gets players normalised mouse position

        if(InteractAction.WasPressedThisFrame())    // checks interact button (E Key)
        {

            Debug.Log("Player clicked E to Interact");
        }

        if(EscapeAction.WasPressedThisFrame())  // checks if Escape has been selected (ESC Key)
        {
            Debug.Log("Player clicked Escape to escape");
        }
    }
    

    private void CheckRightMouseInput() // checks different right mouse inputs
    {
        if(rightMouse.IsPressed())
        {
            RightClickCount ++;
            //Debug.Log("Left Mouse was pressed: current time is: " + LeftClickCount);
        }

        if(rightMouse.WasPressedThisFrame())
        {
            //Debug.Log("Right Mouse was pressed this frame");
        }

        if(rightMouse.WasReleasedThisFrame())
        {
            Debug.Log("Right Mouse was Released, mouse was held for: " + RightClickCount);
            RightClickCount = 0;
        }
    }

    private void CheckLeftMouseInput()  // checks different left mouse inputs
    {
        if(leftMouse.IsPressed())
        {
            LeftClickCount ++;
            //Debug.Log("Left Mouse was pressed: current time is: " + LeftClickCount);
        }

        if(leftMouse.WasPressedThisFrame())
        {
            Debug.Log("Left Mouse was pressed this frame");
        }

        if(leftMouse.WasReleasedThisFrame())
        {
            Debug.Log("Left Mouse was Released, mouse was held for: " + LeftClickCount);
            LeftClickCount = 0;
        }
    }



    private void MovePlayer(Vector2 moveValue) // move to model and view class
    {
        PlayerObject.transform.position += (Vector3)(moveValue * CheckSprint() * 0.01f );
    }

    private float CheckSprint() // checks if the sprint key is pressed, and returns an appropriate value to the movement function
    {
        if(shiftAction.IsPressed())
        {
            Debug.Log("Player is Sprinting");
            return sprintSpeed * moveSpeed;
        }
        
        return moveSpeed;
        
    }





}
