using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReaction : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public Message healthMessage;


    public void Use(int amountToIncrease)
    {
        currentHealth = GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().currentHealth;
        maxHealth = GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().maxHealth;

        if ((currentHealth + amountToIncrease) <= maxHealth)
        {
            GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().currentHealth += amountToIncrease;
        }
        else
        {
            currentHealth = maxHealth;
        }

        GameObject.FindWithTag("Follower").GetComponentInChildren<FollowerHealth>().currentHealth = currentHealth;
        healthMessage.Raise();
    }

}
