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
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.y));
        if(moveDirection == Vector2.zero)
        {
            rigidbodyRef.linearVelocity = Vector2.zero;
            return;    
        }
        
        SetFlip(moveDirection);
        //Debug.Log("Moving player: " + moveDirection);
        rigidbodyRef.MovePosition((Vector2)transform.position + (moveDirection * SprintValue * movementSpeed));

    }

    private void SetFlip(Vector2 moveDirection)
    {
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
