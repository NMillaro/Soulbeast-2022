using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : LootDrop
{
    public float healthIncrease;
    public FloatValue hpBar;

    private void Update()
    {

    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Follower") || other.CompareTag("Player") && !other.isTrigger)
        {

            GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().currentHealth += healthIncrease;
            
            if (GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().currentHealth > hpBar.RuntimeValue)
            {
                GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().currentHealth = hpBar.RuntimeValue;
            }
            lootMessage.Raise();
            Destroy(this.gameObject);
        }
        
    }

}

