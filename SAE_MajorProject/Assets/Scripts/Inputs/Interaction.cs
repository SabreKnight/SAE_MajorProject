using UnityEngine;

public class InteractionManager : MonoBehaviour    // attach to player object
{
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private GameObject heldObject;
    [SerializeField] private Grid grid;
    [SerializeField] private InventoryManager inventory;

    
    private Collider2D[] hitMarkerCollider = new Collider2D[10];

    private Vector3 markerOffset = new Vector3(0.5f, 0.5f, 0);

    public void useleftClickTool()
    {
        Debug.Log("Player attempted to use a tool");
        hitMarkerCollider = Physics2D.OverlapBoxAll(hitMarker.gameObject.transform.position, Vector2.one, 0f);

        foreach(Collider2D coll in hitMarkerCollider)
        {
            if(coll != null)
            {
                Debug.Log(" hit collider has detected; " + coll);
            }
        }

    }
    

    public void MoveHitmarker(Vector2 LookDirection)
    {
        Vector3Int LookVectInt = (Vector3Int)Vector2Int.RoundToInt(LookDirection);
        Vector3Int hitVector =  grid.WorldToCell(this.gameObject.transform.position) + LookVectInt;
        hitMarker.transform.position = (Vector3)hitVector + markerOffset;
    }

    public void useinteractkey()
    {

    }


}
