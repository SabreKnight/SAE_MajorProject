using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerHealth : Health_BaseClass
{
    private InteractionManager interactionManagerRef;
    private AudioManager audioManager;
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

            if(value < 0)
            {
                StartCoroutine(DamageEffect());    
            }

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

    protected override IEnumerator DamageEffect()
    {
        GameManager.Instance.audioManagerSingleton.PlayAudio("Hurt", true);

        Debug.Log("Player has taken damage and is showing a damage effect");
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = BaseColour;
    }

    protected override void CharacterDeath()
    {
        if(charManagerRef == null)
        {
            Debug.Log("No reference for character manager on object");
        }
        Debug.Log(this.gameObject + " character has died, sending to death manager: ");

        charManagerRef.CharacterDied(this);
    }

    public override void CharacterSpawn(I_DeathManager deathManager)
    {
        charManagerRef = deathManager;
        if(deathManager is PlayerRespawn PS)
        {
            transform.position = PS.gameObject.transform.position;
        }
        health = fullhealth;
    }
}
