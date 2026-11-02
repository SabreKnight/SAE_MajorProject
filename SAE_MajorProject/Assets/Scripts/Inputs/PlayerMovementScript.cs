using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbodyRef;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool flipbool;

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
        SetAnimations(moveDirection);

    }

    private void SetAnimations(Vector2 moveDirection)
    {
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.y));
        bool tempbool = flipbool;
        if(moveDirection.x < 0 )
        {
            flipbool = true;
        }
        else
        {
            flipbool = false;
        }

        if(tempbool != flipbool)
        {
            spriteRenderer.flipX = flipbool;
        }
    }
}
