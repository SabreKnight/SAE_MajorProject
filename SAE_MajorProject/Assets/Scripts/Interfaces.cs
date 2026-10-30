using UnityEngine;

public interface I_HitObject
{
    public void HitObjectMethod(Item tool)
    {
        
    }
}

public interface I_InteractObject
{
    public bool InteractObjectMethod()
    {
        return false;
    }

    public int DepositItem(InstantiatedItem item, int panelIndex)
    {

        return 0;
    }
}
