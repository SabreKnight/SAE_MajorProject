using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Health_BaseClass : MonoBehaviour, I_Dammageable, I_HitObject
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Color BaseColour = new Color(1f, 1f, 1f, 1f);
    [SerializeField] protected float fullhealth = 10;
    [SerializeField] protected float health = 10;
    [SerializeField] protected float KnockbackMult = 1f;

    [Header("Death")]
    public float DeathCoolDown = 2f;
    [SerializeField] public I_DeathManager charManagerRef;
    [SerializeField] protected Item DeathDrop;
    [SerializeField] protected int DropAmount;
    [SerializeField] protected int randDropVariation = 3;
    public EnemyActionManager enemyActionManager;

    [Header("Invulnurability")]
    [SerializeField] protected float InvultimerCount = 0.2f;
    [SerializeField] protected float invulCurrentTime;
    [SerializeField] protected bool invulBool;
    


    protected virtual void Update()
    {
        
    }
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

    public void HitObjectMethod(Item Weapon, Vector3 hitDirection)
    {
        Debug.Log("attempting to hit using tool");
        Damaged(Weapon.attackStats, hitDirection);
    }

    public void Damaged(AttackStats attackStats, Vector3 hitDirection)
    {
        Vector3 KnockbackDirection = (transform.position - hitDirection).normalized * attackStats.KnockbackMult;

        //transform.position += KnockbackDirection * KnockbackMult;

        if(invulBool == true)
        {
            Debug.Log("Invulnerability is currently active on: " + this);
            return;    
        }
        

        float DamageValue = attackStats.CalculateAttack(1f);
        Debug.Log(this.gameObject + " took: " + DamageValue + " damage");
        StartCoroutine(InvulTimer());
        HealthGetSet =  DamageValue * -1;
    }

    protected virtual IEnumerator DamageEffect()
    {

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = BaseColour;
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
        Dropitem();
        if(enemyActionManager != null)
        {
            enemyActionManager.ChangeState(3);    
        }

        charManagerRef.CharacterDied(this);
    }

    protected virtual void Dropitem()
    {
        if(DeathDrop != null)
        {
            int newdropAmount = Random.Range(DropAmount - randDropVariation, DropAmount + randDropVariation);
            InstantiatedItem dropitem = new InstantiatedItem(DeathDrop, DropAmount);
            GameObject DropObject = new GameObject("Loot", typeof(PickUpItem));
            int dropDirection = ((int)Random.Range(0,2) * 2) - 1;
            DropObject.GetComponent<PickUpItem>().SetupObject(dropitem, dropDirection, transform.position);
        }
    }

    public virtual void CharacterSpawn(I_DeathManager deathManager)
    {
        charManagerRef = deathManager;
        GetComponent<SpriteRenderer>().enabled = true;
        health = fullhealth;

        if(enemyActionManager != null)
        {
            enemyActionManager.ChangeState(0);
        }

        Debug.Log(this.gameObject + " has been spawned with: " + health + " health death manager as: " + charManagerRef);
    }
}
