using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisMonster : MonoBehaviour
{
    public Monster thisMonster;
    public UniqueStats thisMonsterStats;

    [System.Serializable]
    public class UniqueStats
    {
        public string nickname;
        public int currentHP;
        public int level;
        public int exp;
        public int companionship;

        //currentHP = thisMonster.
    }
}
