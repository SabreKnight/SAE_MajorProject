using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PickUpItem : MonoBehaviour
{

    [SerializeField] private InstantiatedItem instantiatedItemRef;

    [Header("Pickup Cooldown")]
    [SerializeField] private float CooldownTimer = 2f;
    private bool Pickupbool = false; 

    private SpriteRenderer spriteRenderer;

    [Header ("Drop Animation")]

    [SerializeField] private AnimationCurve dropCurve;
    private Vector3 startingposition;
    [SerializeField] private float lerpScale = 0.15f;
    [SerializeField] private float HorizontalScale = 0.25f;
    [SerializeField] private int DropDirection;

    [Header ("Pick Up Collision")]

    [SerializeField] private GameObject playerRef = null;
    private int colliderCheckInterval = 0;
    [SerializeField] private int colliderCheckThreshold = 5;
    [SerializeField] private float ColliderCircleRadius = 2f;
    [SerializeField] private float PickupSpeed = 0.01f;
    [SerializeField] private float PickupDistance = 0.5f;
    

    // void OnEnable()
    // {
    //     Pickupbool = false;
    //     StartCoroutine(DropAnimation());
    // }

    void Update()
    {
        if(Pickupbool)
        {
            if(playerRef == null)
            {
                ColliderCheck();
            }
            else
            {
                ActivePickup();
            }
        }
    }

    private void ColliderCheck()
    {
        if(colliderCheckInterval !=  colliderCheckThreshold)
        {
            colliderCheckInterval ++;
            return;
        }

        colliderCheckInterval = 0;
        Collider2D[] colliderCheck =  Physics2D.OverlapCircleAll(transform.position, ColliderCircleRadius);

        foreach(Collider2D collider in colliderCheck)
        {
            if(collider != null && collider.gameObject.CompareTag("Player"))
            {
                playerRef = collider.gameObject;
                Debug.Log("Player located: " + playerRef);
            }
        }
    }

    private IEnumerator DropAnimation()
    {
        dropCurve = GameManager.Instance.DroppedItemAnimation;
        Debug.Log("Starting drop animation");
        startingposition = transform.position;
        float lerpvalue = 0.00f;
        while (lerpvalue < 1)
        {
            startingposition += new Vector3(lerpScale * lerpvalue * HorizontalScale, lerpScale * dropCurve.Evaluate(lerpvalue), 0);
            lerpvalue += 0.01f;

            transform.position = startingposition;
            Debug.Log("Lerp scale: " + lerpScale + " lerp value: " + lerpvalue);
            Debug.Log("current drop position: " + startingposition + " current lerp value is: " + lerpvalue + " and current drop curve value is: " + dropCurve.Evaluate(lerpvalue));
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(CooldownTimer);
        Pickupbool = true;
        Debug.Log("Player can now pick up: " + this.gameObject.name);
    }

    private void ActivePickup()
    {
        Vector3 playerDirection = playerRef.transform.position - this.transform.position;
        
        transform.position += playerDirection.normalized * PickupSpeed;

        if(Vector3.Distance(playerRef.transform.position, transform.position) <= PickupDistance)
        {
            int returnvalue = playerRef.GetComponent<InteractionManager>().inventory.AddItemToInventory(instantiatedItemRef);

            if(returnvalue > 0)
            {
                instantiatedItemRef.currentStackSize = returnvalue;
                playerRef = null;
                Debug.Log(this.gameObject.name + " Could not be picked up");

            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }
    }

    public void SetupObject(InstantiatedItem newitem, float dropdirection, Vector3 Vect)
    {
        transform.position = Vect;
        instantiatedItemRef = newitem;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = instantiatedItemRef.itemType.inventorySprite;

        if(dropdirection != 0)
        {
            HorizontalScale = HorizontalScale * dropdirection;  
        }

        Pickupbool = false;
        StartCoroutine(DropAnimation());

    }


}
