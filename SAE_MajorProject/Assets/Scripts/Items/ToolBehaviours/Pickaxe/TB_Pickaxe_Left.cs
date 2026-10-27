using UnityEngine;

public class TB_Pickaxe_Left : ToolBehaviours_BaseClass
{

    public override I_HitObject[] ToolClickMethod(Vector2 hitMarker)
    {
        UnityEngine.Debug.Log("player clicked to use the pickaxe");

        hitobjectArray = new I_HitObject[10];
        hitArray = new Collider2D[10];

        Debug.Log("hitmarker position: " + hitMarker);

        hitArray = Physics2D.OverlapBoxAll(hitMarker + (Vector2)transform.position, Vector2.one, 0f);

        if(hitArray.Length <= 0 || hitArray == null)
        {
            Debug.Log("No hits found on tool hit array");
            return null;
        }

        for(int i = 0; i < hitobjectArray.Length; i ++)
        {
            I_HitObject hitObj = hitArray[i].gameObject.GetComponent(typeof(I_HitObject)) as I_HitObject;

            if(hitObj != null)
            {
                Debug.Log(" hit collider has detected; " + hitArray[i].gameObject);
                hitobjectArray[i] = hitObj;
            }
            else
            {
                Debug.Log("no hittable object has been detected on index: " + i);
            }
        }

        return hitobjectArray;
    } 
}
