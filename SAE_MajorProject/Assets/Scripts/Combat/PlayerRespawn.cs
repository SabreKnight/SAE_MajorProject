using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerRespawn : MonoBehaviour, I_DeathManager
{
    [SerializeField] private GameObject RespawnUI;
    [SerializeField] private Health_BaseClass playerhealth;
    public void CharacterDied(Health_BaseClass characterHealth)
    {
        Characterdeath();
    }

    private void Characterdeath()
    {
        RespawnUI.SetActive(true);
        GameManager.Instance.inputController.OverrideGetSet = true;

    }

    public void RespawnButton()
    {
        RespawnUI.SetActive(false);
        GameManager.Instance.inputController.OverrideGetSet = false;
        playerhealth.CharacterSpawn(this);
    }

    private void OnEnable()
    {
        playerhealth.charManagerRef = this;
        RespawnUI.SetActive(false);
    }

}
