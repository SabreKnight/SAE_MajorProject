using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// input manager is a controller in controller, model, view networking framework
public class InputManager_Cont : MonoBehaviour
{
    public InputActionAsset InputAction;
    public InteractionManager interactionScript;
    [SerializeField] private UIManager UIManagerScript;
    [HideInInspector] public bool UIOverride = false;

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

    private InputAction scrollInput;
    [SerializeField] private float scrollValue;

    private InputAction InteractAction; // interact input action (E Key)
    private InputAction EscapeAction;   // Escape Input action (ESC Key)
    private InputAction InventoryAction;
    private InputAction dropInput;

# endregion

# region PointingDirection

    private InputAction lookAction;
    private InputAction MousePositionAction;
    [SerializeField] private Vector2 Mouseposition;
    [SerializeField] private Vector2 MouseDirection = Vector2.zero;
    [SerializeField] private Vector2 TrueLookdirection;
    private int UpdateCounter = 0;

# endregion
    
    void Start()
    {
        if(UIManagerScript == null)
        {
            UIManagerScript = GameManager.Instance.UIManagerSingleton;
        }
    }
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
        lookAction = InputSystem.actions.FindAction("Look Direction");
        MousePositionAction = InputSystem.actions.FindAction("Mouse Position");
        InventoryAction = InputSystem.actions.FindAction("Inventory");
        scrollInput = InputSystem.actions.FindAction("Scroll");
        dropInput = InputSystem.actions.FindAction("Drop Item");

        if(UIManagerScript == null)
        {
            UIManagerScript = GameManager.Instance.UIManagerSingleton;
        }

        SetupPanels();
    }

    void SetupPanels()
    {
        UIManagerScript.SetUpPanel();
        UIOverride = false;
    }

    private void Update()
    {
        InventoryToggle();

        if(UIOverride == false) // inputs will be undetected if Override is active
        {
            CheckMisc();
            CheckLeftMouseInput();
            CheckRightMouseInput();
            UpdateModel();
        }
    }

    private void FixedUpdate()
    {
        MousePointer();
    }

    private void UpdateModel()
    {
        interactionScript.MoveHitmarker(TrueLookdirection);
    }
    
    private void InventoryToggle()
    {
        if(InventoryAction.WasPressedThisFrame())
        {
            UIOverride = UIManagerScript.InventoryinputButton();
        }

        if(InteractAction.WasPressedThisFrame())    // checks interact button (E Key)
        {
            UIOverride = interactionScript.InteractkeyMethod();
        }

    }
    private void CheckMisc()    // checks any miscellanious inputs short enough for just 1 functions
    {
        //Checks Movement values
        moveValue = moveAction.ReadValue<Vector2>();    // checks movement values for the player
        MovePlayer(moveValue);  // calls movement script

        if(EscapeAction.WasPressedThisFrame())  // checks if Escape has been selected (ESC Key)
        {
            Debug.Log("Player clicked Escape to escape");
        }

        if(dropInput.WasPressedThisFrame())  // checks if Escape has been selected (ESC Key)
        {
            interactionScript.DropheldObject();
            Debug.Log("Player clicked Q to Drop Item");
        }

        scrollValue = scrollInput.ReadValue<float>();

        if(scrollValue != 0)
        {
            //Debug.Log("Player scroll value is equal to: " + scrollValue);
            UIManagerScript.ScrollHotInventory(scrollValue);
        }
    }

    private void MousePointer()
    {
        if(UpdateCounter != 8)  // games slows due to too many updates, this slows the updates down.
        {
            UpdateCounter ++;
            return;
        }
        else
        {
            UpdateCounter = 0;
        }

        // this gets the direction based on the mouse position on the screen
        //Vector2 TempMouseVal = (lookAction.ReadValue<Vector2>() - new Vector2(Screen.width / 2f, Screen.height / 2f));

        // this gets the direction based on the mouse position in relation to the player character
        Vector2 TempMouseVal = (Camera.main.ScreenToWorldPoint(lookAction.ReadValue<Vector2>()) - interactionScript.gameObject.transform.position);
        
        if(MouseDirection != TempMouseVal) // gets players normalised mouse position
        {
            MouseDirection = TempMouseVal;

            TrueLookdirection = MouseDirection.normalized;
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
            interactionScript.LeftClick();
           // Debug.Log("Left Mouse was pressed this frame");
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
        if(moveValue != Vector2.zero)
        {
            TrueLookdirection = moveValue;
        }
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
