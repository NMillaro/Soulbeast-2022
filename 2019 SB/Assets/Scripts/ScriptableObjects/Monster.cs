using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Monster : ScriptableObject
{
    public string monsterName;
    public Sprite monsterImage;
    public RuntimeAnimatorController monsterAnimator;
    public int monsterNumber;
    public MonsterType type1;
    public MonsterType type2;
    public Rarity rarity;
    public int expGroup;
    public List<GenericTech> learnedTechs = new List<GenericTech>();
    public BaseStats monsterStats;

    [SerializeField] private bool canAscend;
    [SerializeField] private MonsterAscension AscendTo;

}

public enum Rarity
{
    VeryCommon,
    Common,
    Uncommon,
    Rare,
    VeryRare
}

public enum MonsterType
{
    Neutral,
    Earth,
    Air,
    Fire,
    Water,
    Metal,
    Magic,
    Light,
    Dark,
    Chaos,
    Energy  
}



[System.Serializable]
public class MonsterAscension
{
    public Monster nextAscension;
    public int AscensionLevel;
}

[System.Serializable]
public class BaseStats
{
    public int HPStat;
    public int StrengthStat;
    public int MagicStat;
    public int StaminaStat;
    public int WillpowerStat;
    public int IntelligenceStat;
    public int WisdomStat;
    public float SpeedStat;
}
