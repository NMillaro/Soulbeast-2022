using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{
    public Slider playerHealthFill;
    public GameObject healthSlider;
    public float monsterCurrentHealth;
    public FloatValue HPBar;
    public float maxHealth;


    void Start()
    {
        Invoke("UpdateHealth", 0.1f);
        //maxHealth = GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().maxHealth.initialValue;
       monsterCurrentHealth = maxHealth;
        //InitHealth();
    }

    public void InitHealth()
    {
        playerHealthFill.value = 1;
    }

    public void UpdateHealth()
    {
        maxHealth = GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().maxHealth;
        HPBar.RuntimeValue = maxHealth;
        monsterCurrentHealth = GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().currentHealth;
        float tempHealth = monsterCurrentHealth;
        playerHealthFill.value = tempHealth / maxHealth;

        if (playerHealthFill.value <= 0)
        {
            healthSlider.SetActive(false);
        }

    }
}
