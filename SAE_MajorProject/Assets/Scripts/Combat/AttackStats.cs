using UnityEngine;


[System.Serializable]
public class AttackStats
{
    public float damageStat = 1;
    public float CritChance = 0.2f;
    public float critMultiplier = 2;
    public float altmultiplier = 1;
    public float KnockbackMult = 1;
    public Vector2 AttackRange = Vector2.one;
    public Vector2 AttackSize = Vector2.one;

    public float CalculateAttack(float altmult)
    {
        float damage = damageStat * altmult;

        if(UnityEngine.Random.Range(0f, 1f) <= CritChance)
        {
            damage *= critMultiplier;
        }

        return damage;
    }
}
