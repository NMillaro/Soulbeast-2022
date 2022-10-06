using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyHealth : GenericHealth
{
    [Header("Game Manager")]
    public GameManager gm;
    public int baseExpYield;
    public int finalExpYield;
    public int level;

    [Header("Death Effects")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f; //Delay before effect destroyed
    public LootTable thisLoot;

    [Header("Death Message")]
    public Message roomMessage;

    [Header("Damage Message")]
    [SerializeField] private Message damageMessage = null;


    protected override void OnEnable()
    {
        Invoke("InitialiseMonStats", 0.1f);
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    protected void Start()
    {
        Invoke("InitialiseMonStats", 0.1f);
        
    }

    protected void CalculateExp()
    {
        baseExpYield = MonsterData.BaseExpYield[GetComponentInParent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonster.rarity.ToString()];
        level = GetComponentInParent<MonsterStats>().currentLevel;
        finalExpYield = baseExpYield * level;
    }

    public override void Damage(float amountToDamage)
    {
        base.Damage(amountToDamage);

        if (damageMessage != null)
        {
            damageMessage.Raise();
        }

        if (currentHealth <= 0)
        {
            if (roomMessage != null)
            {
                roomMessage.Raise();
            }
            DeathEffect();
            MakeLoot();
            gm.levelSystem.AddExperience(finalExpYield);
            Debug.Log("Exp: " + finalExpYield);
           
            
            if (gameObject.transform.parent.GetComponentInChildren<CaptureAttempt>())
            {
                if (gameObject.transform.parent.GetComponentInChildren<CaptureAttempt>().isCapturing == true)
                {
                    GameObject NewMonster = Instantiate(transform.parent.gameObject.GetComponent<MonsterStats>().thisMonster);
                    NewMonster.GetComponent<ThisMonster>().thisMonsterStats.level = gameObject.transform.parent.GetComponent<MonsterStats>().currentLevel;
                    gm.GetComponent<GameManager>().ownedMonsters.AddMonster(NewMonster);
                    gameObject.transform.parent.GetComponentInChildren<CaptureAttempt>().isCapturing = false;
                    gameObject.transform.parent.GetComponentInChildren<ContextClue>().ContextOff();
 
                }
            }

            transform.parent.gameObject.SetActive(false);
        }
        
       // StartCoroutine(IframeFlashCo());
        

    }

    private void InitialiseMonStats()
    {
        maxHealth = GetComponentInParent<MonsterStats>().HPStat;
        currentHealth = maxHealth;
        CalculateExp();
    }
    

    private void MakeLoot()
    {
        if (thisLoot != null)
        {
            LootDrop current = thisLoot.LootsDrop();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    private void DeathEffect()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }

    }   
}
