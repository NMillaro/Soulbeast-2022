using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class MonsterInventory : ScriptableObject
{
    public GameObject currentMonster;
    public List<GameObject> monsters = new List<GameObject>();

    public void AddMonster(GameObject monsterToAdd)
    {
        
        if (monsters.Count <= 2)
        {
            monsters.Add(monsterToAdd);
        }
        
    }
}
