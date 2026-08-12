using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager_Cont : MonoBehaviour
{
    public InputActionAsset InputAction;
    public InputAction moveAction;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject PlayerObject;
    //[SerializeField] private Rigidbody2D PlayerRigid;
    [SerializeField] private Vector2 moveValue;
    
    void OnEnable()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        MovePlayer(moveValue);

    }

    private void MovePlayer(Vector2 moveValue)
    {
        PlayerObject.transform.position += (Vector3)(moveValue * moveSpeed);
        //PlayerRigid.AddForce(moveValue * moveSpeed);
    }
}
