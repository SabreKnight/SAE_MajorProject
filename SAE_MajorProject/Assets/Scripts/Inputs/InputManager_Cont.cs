using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager_Cont : MonoBehaviour
{
    public InputActionAsset InputAction;

# region Movement
    private InputAction moveAction;
    [Header ("Movement")]
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private Vector2 moveValue;
# endregion
    
    void OnEnable()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        MovePlayer(moveValue);

    }

    private void MovePlayer(Vector2 moveValue) // move to model and view class
    {
        PlayerObject.transform.position += (Vector3)(moveValue * moveSpeed);
    }





}
