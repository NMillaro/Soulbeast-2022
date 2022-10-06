using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private int level;
    private int monsterNumber;
    private int exp;
    public int currentMonster;
    public Dictionary<int, Dictionary<string, object>> OwnedMonsters
        = new Dictionary<int, Dictionary<string, object>>();

    public PlayerData(GameManager gm)
    {

        for (int i = 0; i < gm.ownedMonsters.monsters.Count; i++)
        {
            monsterNumber = gm.ownedMonsters.monsters[i].GetComponent<ThisMonster>().thisMonster.monsterNumber;
            level = gm.ownedMonsters.monsters[i].GetComponent<ThisMonster>().thisMonsterStats.level;
            exp = gm.ownedMonsters.monsters[i].GetComponent<ThisMonster>().thisMonsterStats.exp;
            OwnedMonsters.Add(i, new Dictionary<string, object>());
            OwnedMonsters[i].Add("number", (int)monsterNumber);
            OwnedMonsters[i].Add("level", (int)level);
            OwnedMonsters[i].Add("exp", (int)exp);

            Debug.Log("Monster saved: " + gm.ownedMonsters.monsters[i].GetComponent<ThisMonster>().thisMonster.monsterName);

        }

        //records currently active monster to remain as main monster
        for (int i = 0; i < gm.ownedMonsters.monsters.Count; i++)
        {
            if (gm.ownedMonsters.monsters[i] == gm.ownedMonsters.currentMonster)
            {
                currentMonster = i;
                Debug.Log("Active Monster: " + gm.ownedMonsters.monsters[i].GetComponent<ThisMonster>().thisMonster.monsterName);
            }
        }
    }

   
}
