using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : LootDrop
{
    public Inventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        lootMessage.Raise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Follower") || other.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.coins += 1;
            lootMessage.Raise();
            Destroy(this.gameObject);
        }

    }

}
