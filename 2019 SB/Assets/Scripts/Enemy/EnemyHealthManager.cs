using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{
    public Slider enemyHealthFill;
    //public GameObject healthSlider;
    public float enemyCurrentHealth;
    public float enemyMaxHealth;
   // public FloatValue HPBar;
    public float tempHealth;
    public GameObject healthBar;
    public float getHealth;

    void Start()
    {
        InitHealth();

    }

    void FetchHealth()
    {
        getHealth = gameObject.GetComponentInParent<GenericHealth>().currentHealth;
    }

    public void InitHealth()
    {
        enemyHealthFill.value = 1;
    }


    void Update()
    {
        if (enemyCurrentHealth == enemyMaxHealth)
        {
            healthBar.SetActive(false);
        }
        else
        {
            healthBar.SetActive(true);
        }
        FetchHealth();
        enemyMaxHealth = gameObject.GetComponentInParent<MonsterStats>().HPStat;
        enemyCurrentHealth = getHealth;
        enemyHealthFill.value = enemyCurrentHealth / enemyMaxHealth;
    }
}
