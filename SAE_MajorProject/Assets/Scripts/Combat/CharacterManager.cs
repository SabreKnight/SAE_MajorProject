using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour, I_DeathManager
{
    private int managerCount = 3;
    private int ActiveCharacterCount = 2;
    [SerializeField] private List<Health_BaseClass> CharacterQueue;
    private float playerCheckRadius = 10f;
    [SerializeField] private float CharacterSpawnOffset = 5;
    private float SpawnInterval = 4;

    private bool activeQueue = true;

    public IEnumerator CharacterDied(Health_BaseClass characterHealth)
    {
        Debug.Log(characterHealth.gameObject + " Has started death cooldown");
        float cooldown = characterHealth.DeathCoolDown;
        yield return new WaitForSeconds(1f);
        CharacterQueue.Add(characterHealth);
        Debug.Log(characterHealth.gameObject + " Has entered the spawn pool");
        //yield return null;
    }

    private IEnumerator SpawnCharacter()
    {
        activeQueue = true;
        Health_BaseClass characterHealth = CharacterQueue[0];
        characterHealth.gameObject.transform.position = new Vector3(transform.position.x + Random.Range(-CharacterSpawnOffset, CharacterSpawnOffset), transform.position.y + Random.Range(-CharacterSpawnOffset, CharacterSpawnOffset), transform.position.z);
        Debug.Log("Spawning " + characterHealth.gameObject + " at: " + characterHealth.gameObject.transform.position);
        characterHealth.CharacterSpawn(this as I_DeathManager);
        CharacterQueue.RemoveAt(0);
        activeQueue = false;
        yield return null;
    }

    private void Start()
    {
        activeQueue = true;
        foreach(Health_BaseClass character in CharacterQueue)
        {
            character.charManagerRef = this as I_DeathManager;
            character.GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log(character.gameObject + " has been initialised in character manager");
        }
        activeQueue = false;
    }

    private void Update()
    {
        if(!activeQueue && CharacterQueue.Count > managerCount - ActiveCharacterCount)
        {
            StartCoroutine(SpawnCharacter());
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.Brown;
        
        //Gizmos.DrawWireSphere(transform.position, radius);
    }
}
