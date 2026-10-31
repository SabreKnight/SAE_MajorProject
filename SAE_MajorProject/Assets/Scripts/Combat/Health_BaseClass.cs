using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Health_BaseClass : MonoBehaviour, I_HitObject
{
    [SerializeField] protected float fullhealth = 10;
    [SerializeField] protected float health = 10;

    [Header("Death")]
    public float DeathCoolDown = 2f;
    [SerializeField] public I_DeathManager charManagerRef;

    [Header("Invulnurability")]
    [SerializeField] protected float InvultimerCount = 0.2f;
    [SerializeField] protected float invulCurrentTime;
    [SerializeField] protected bool invulBool;

    protected virtual float HealthGetSet
    {
        get
        {
            return health; 
        }

        set
        {
            health += value;

            Mathf.Clamp(health, 0f, fullhealth);

            if(health <= 0f)
            {
                health = fullhealth;
                CharacterDeath();
            }
        }
    }

    public virtual void HitObjectMethod(Item Weapon, Vector3 hitDirection)
    {
        HitObjectMethod(Weapon, 1f, hitDirection);
    }

    public virtual void HitObjectMethod(Item weapon, float altmult, Vector3 hitDirection)
    {
        Vector3 KnockbackDirection = (transform.position - hitDirection).normalized * weapon.KnockbackMult;

        transform.position += KnockbackDirection;

        if(invulBool == true)
        {
            Debug.Log("Invulnerability is currently active on: " + this);
            return;    
        }

        Debug.Log("using: " + weapon + " in attack method");

        float damage = weapon.damageStat * altmult;

        if(Random.Range(0f, 1f) <= weapon.CritChance)
        {
            damage *= weapon.critMultiplier;
        }
        
        Debug.Log(this.gameObject + " took: " + damage + " damage from: " + weapon.ItemName);
        StartCoroutine(InvulTimer());
        HealthGetSet = damage * -1;
    }

    protected virtual IEnumerator InvulTimer()
    {
        invulBool = true;
        invulCurrentTime = InvultimerCount;

        while(invulCurrentTime >= 0)
        {
            invulCurrentTime -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        invulBool = false;
        yield return null;
    }

    protected virtual void CharacterDeath()
    {
        if(charManagerRef == null)
        {
            Debug.Log("No reference for character manager on object");
        }
        Debug.Log(this.gameObject + " character has died, sending to death manager: ");
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(charManagerRef.CharacterDied(this));
    }

    public virtual void CharacterSpawn(I_DeathManager deathManager)
    {
        charManagerRef = deathManager;
        GetComponent<SpriteRenderer>().enabled = true;
        health = fullhealth;

        Debug.Log(this.gameObject + " has been spawned with: " + health + " health death manager as: " + charManagerRef);
    }
}
