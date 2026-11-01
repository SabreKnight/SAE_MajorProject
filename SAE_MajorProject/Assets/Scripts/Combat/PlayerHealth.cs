using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerHealth : Health_BaseClass
{
    private InteractionManager interactionManagerRef;
    private float TotalHealCooldown = 5f;
    private float CurrentHealCooldown;
    private bool activeHealBool;
    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText; 
    
    void Start()
    {
        HealthGetSet = 0;
    }
    protected override float HealthGetSet
    {
        get
        {
            return health; 
        }

        set
        {
            health += value;
            Mathf.Clamp(health, 0f, fullhealth);

            healthSlider.value = health;
            healthText.text = "Health: " + health.ToString();

            if(health <= 0f)
            {
                health = fullhealth;
                CharacterDeath();
            }
        }
    }

    protected override void Update()
    {
        if(HealthGetSet < fullhealth && activeHealBool == false)
        {
            StartCoroutine(Passiveheal());
        }
    }

    private IEnumerator Passiveheal()
    {
        activeHealBool = true;
        CurrentHealCooldown = 0f;
        while(CurrentHealCooldown < TotalHealCooldown)
        {
            yield return new WaitForSeconds(1f);
            CurrentHealCooldown += 1f;
        }
        Debug.Log("Player has healed a health point");

        HealthGetSet = 1;
    }
}
