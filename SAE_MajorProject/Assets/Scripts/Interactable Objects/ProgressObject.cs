using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(Collider2D))]
public class ProgressObject : MonoBehaviour, I_InteractObject
{
    public void InteractObjectMethod()
    {
        Debug.Log("Progress object has been interacted with");
    }

    private void ActivateProgressObject()
    {
        
    }

    private void DeactivateProgressObject()
    {
        
    }
}
