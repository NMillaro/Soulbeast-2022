using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public LootDrop thisLoot;
    public int lootChance;
}


[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public Loot[] loots;

    public LootDrop LootsDrop()
    {
        float cumilativeProb = 0;
        float currentProb = Random.Range(0, 100);

        for (int i = 0; i < loots.Length; i++)
        {
            cumilativeProb += loots[i].lootChance;
            if(currentProb <= cumilativeProb)
            {
                return loots[i].thisLoot;
            }
        }

        return null;
    }

}
