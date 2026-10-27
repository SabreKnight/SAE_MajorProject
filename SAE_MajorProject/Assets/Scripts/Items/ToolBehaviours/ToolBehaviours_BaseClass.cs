using UnityEngine;

public abstract class ToolBehaviours_BaseClass : MonoBehaviour
{
    protected I_HitObject[] hitobjectArray = new I_HitObject[10];
    protected Collider2D[] hitArray = new Collider2D[10];
    [SerializeField] protected InteractionManager interactionManagerRef;
    public bool PrimaryAction; 

    protected virtual void OnEnable()  // gets the reference to the interaction manager reference
    {
        if(interactionManagerRef == null)
        {
            interactionManagerRef = GameManager.Instance.inputController.interactionScript;
        }
    }

    // tool method for when the appropriate tool use button is clicked
    public virtual I_HitObject[] ToolClickMethod(Vector2 hitMarker)
    {
        UnityEngine.Debug.Log("player clicked to use this tool");
        return null;
    }

    // tool method for when the tool use button is held down, and how long for
    public virtual I_HitObject[] ToolHoldMethod(int timeHeld)
    {
        UnityEngine.Debug.Log("player is holding to use this tool");
        return null;
    }

    // Tool method for when the tool use button is released, and how long it was held for
    public virtual I_HitObject[] ToolReleaseMethod(int TimeHeld)
    {
        UnityEngine.Debug.Log("player is let go to use this tool");
        return null;
    }
}
