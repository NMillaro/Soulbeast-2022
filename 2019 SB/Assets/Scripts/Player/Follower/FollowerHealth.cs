using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerHealth : GenericHealth
{
    [SerializeField] private Message healthMessage = null;
    [SerializeField] private Collider2D triggerCollider = null;
    [SerializeField] private GameObject player;

    [Header("Game Variables")]
    private GameObject gameManager;
    private float gameSpeed;

    [Header("Damage Message")]
    public Message damageMessage;

    private bool isRegenHealth = false;
    private float timestamp = 0.0f;

    protected override void OnEnable()
    {
        triggerCollider = GetComponent<Collider2D>();
        Invoke("InitialiseHealth", 0.1f);
        player = GameObject.FindWithTag("Player");
        gameManager = GameObject.FindWithTag("GameManager");

    }


    private void Update()
    {
        gameSpeed = gameManager.GetComponent<GameManager>().GameSpeed.RuntimeValue;

        if (currentHealth != maxHealth && Time.time > (timestamp + 10.0f) && gameSpeed != 0)
        {
            if (!isRegenHealth)
            {
                StartCoroutine(RegainHealthOverTime());
            }

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        if (player.GetComponent<PlayerMain>().currentState == CharacterState.combat)
        {
            timestamp = Time.time;
        }
        //if (isRegenHealth)
        //{
        //    healthMessage.Raise();
        //}
    }

    public void UpdateHealth()
    {
        maxHealth = GetComponentInParent<MonsterStats>().HPStat;
    }

    public void InitialiseHealth()
    {
        maxHealth = GetComponentInParent<MonsterStats>().HPStat;
        currentHealth = maxHealth;
    }

    public override void Damage(float amountToDamage)
    {
        timestamp = Time.time;
        base.Damage(amountToDamage);
       // maxHealth.RuntimeValue = currentHealth;
        healthMessage.Raise();
        damageMessage.Raise();
    }

    private IEnumerator RegainHealthOverTime()
    {
        isRegenHealth = true;

        while ((currentHealth < maxHealth) && (Time.time > (timestamp + 10.0f)) 
            && (player.GetComponent<PlayerMain>().currentState != CharacterState.interact) 
            && (player.GetComponent<PlayerMain>().currentState != CharacterState.combat))
        {
            currentHealth += GetComponentInParent<MonsterStats>().HPStat / 10;
            healthMessage.Raise();

            yield return new WaitForSeconds(1);
        }

        isRegenHealth = false;
    }

}
