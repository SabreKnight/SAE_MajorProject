using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbodyRef;
    [SerializeField] private float movementSpeed = 1f;

    void Awake()
    {
        if(rigidbodyRef == null)
        {
            rigidbodyRef = GetComponent<Rigidbody2D>();
        }
    }

    public void MovePlayer(Vector2 moveDirection, float SprintValue)
    {
        if(moveDirection == Vector2.zero)
        {
            rigidbodyRef.linearVelocity = Vector2.zero;
            return;    
        }

        Debug.Log("Moving player: " + moveDirection);
        rigidbodyRef.MovePosition((Vector2)transform.position + (moveDirection * SprintValue * movementSpeed));

    }
}
