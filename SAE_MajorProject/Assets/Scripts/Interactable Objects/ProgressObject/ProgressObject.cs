using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProgressObject : MonoBehaviour, I_InteractObject
{
    [SerializeField] private UIManager UIManagerRef;
    private bool activeInteraction = false;
    [SerializeField] public List<QuotaClass> ProgressQuotaList;
    [SerializeField] public QuotaClass currentQuota;
    [SerializeField] private int currentQuestindex = 0;
    private bool achievedQuota = false;

    private void OnEnable()
    {
        UIManagerRef = GameManager.Instance.UIManagerSingleton;
    }

    public bool InteractObjectMethod()
    {
        Debug.Log("Progress object has been interacted with");
        
        if(UIManagerRef.InteractPanelButton(this))
        {
            activeInteraction = true;
            ActivateProgressObject();
            return true;
        }
        else
        {
            activeInteraction = false;
            DeactivateProgressObject();
            return false;
        }
    }

    private void ActivateProgressObject()
    {
        SetQuest();
        Debug.Log("object activated");
    }

    private void DeactivateProgressObject()
    {
        if(achievedQuota == true)
        {
            FinishCurrentQuota();
        }

        Debug.Log("object De-activated");

        UIManagerRef.CloseInteractPanel();
    }

    private void SetQuest()
    {
        if(currentQuestindex >= ProgressQuotaList.Count)
        {
            FinishQuotaList();   
            return; 
        }

        currentQuota = ProgressQuotaList[currentQuestindex];
        currentQuota.completedquotaArray = new List<bool>();

        for(int i = 0; i <  UIManagerRef.UI_InteractPanels.Count; i++)
        {
            if(i >= currentQuota.quotaitemList.Count)
            {
                UIManagerRef.UI_InteractPanels[i].EmptyPanel();
            }
            else
            {
                UIManagerRef.UI_InteractPanels[i].UpdatePanel(currentQuota.quotaitemList[i]);
                if(currentQuota.quotaitemList[i].currentStackSize >= currentQuota.quotaitemList[i].quotaCount)
                {
                    currentQuota.completedquotaArray.Add(true);
                }
                else
                {
                    
                currentQuota.completedquotaArray.Add(false);
                }
            }
        }

        for(int i = 0; i < UIManagerRef.UI_rewardPanels.Count; i++) // shows all item rewards
        {
            if(i >= currentQuota.RewardItemList.Count)
            {
                UIManagerRef.UI_rewardPanels[i].gameObject.SetActive(false);
            }
            else
            {
                UIManagerRef.UI_rewardPanels[i].gameObject.SetActive(true);
                UIManagerRef.UI_rewardPanels[i].sprite = currentQuota.RewardItemList[i].itemType.inventorySprite;
            }
        }
    }

    private void FinishQuotaList()
    {
        Debug.Log("Player has won the game");
    }

    private void FinishCurrentQuota()
    {
        Debug.Log("Giving quota rewards");
        foreach(InstantiatedItem item in currentQuota.RewardItemList)
        {
            GameObject DropObject = new GameObject("Progression reward: " + item.itemType.ItemName, typeof(PickUpItem));
            int dropDirection = ((int)Random.Range(0,2) * 2) - 1;
            DropObject.GetComponent<PickUpItem>().SetupObject(item , dropDirection, transform.position);
        }
        
        currentQuota = null;
        currentQuestindex ++;
    }

    public int DepositItem(InstantiatedItem item, int panelIndex)
    {

        InstantiatedItem Quotaitem = ProgressQuotaList[currentQuestindex].quotaitemList[panelIndex];
        if(item.itemType == Quotaitem.itemType)
        {
            Quotaitem.currentStackSize += item.currentStackSize;
            
            if(Quotaitem.currentStackSize >= Quotaitem.quotaCount)
            {
                int leftover = Quotaitem.currentStackSize - Quotaitem.quotaCount;
                Quotaitem.currentStackSize = Quotaitem.quotaCount;
                currentQuota.completedquotaArray[panelIndex] = true;
                CheckQuotas();
                UIManagerRef.UI_InteractPanels[panelIndex].UpdatePanel(currentQuota.quotaitemList[panelIndex]);

                Debug.Log("Quota has been fulfilled");
                return leftover;
            }

            UIManagerRef.UI_InteractPanels[panelIndex].UpdatePanel(Quotaitem);
            Debug.Log("depositing all " + item.currentStackSize + " of " + item.itemType);
            return 0;
        }
        else
        {
            Debug.Log("Deposited item is not that same type as the quota type");
            return item.currentStackSize;
        }
    }

    private void CheckQuotas()
    {
        foreach(bool check in currentQuota.completedquotaArray)
        {
            if(check == false)
            {
                Debug.Log("Not all quotas fulfilled");
                return;
            }
        }

        Debug.Log("All quotas for this progression level acheived");
        achievedQuota = true;

    }
}
