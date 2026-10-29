using Unity.VisualScripting;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TB_Harvest_Left : ToolBehaviours_BaseClass
{
    public override List<I_HitObject> ToolClickMethod(Vector2 hitMarker)
    {
        //Debug.Log("player clicked to use the pickaxe");

        hitobjectArray = new List<I_HitObject>();
        hitArray = new Collider2D[10];

        //Debug.Log("hitmarker position: " + hitMarker);

        hitArray = Physics2D.OverlapBoxAll(hitMarker, Vector2.one, 0f);

        if(hitArray.Length <= 0 || hitArray == null)
        {
            //Debug.Log("No hits found on tool hit array");
            return hitobjectArray;
        }

        int arraylength = hitArray.Length;

        Debug.Log("base hit array length is: " + hitobjectArray.Count);

        foreach(Collider2D hit in hitArray)
        {
            
            if(hit.gameObject.TryGetComponent<I_HitObject>(out I_HitObject hitObj) && hit != null)
            {
                hitobjectArray.Add(hitObj);
                Debug.Log("Added: " + hitObj + " to list of hit objects");
                continue;
            }
        }

        Debug.Log("new hit array length is: " + hitobjectArray.Count);
        return hitobjectArray;
    } 
}
