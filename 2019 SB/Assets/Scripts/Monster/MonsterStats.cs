using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    public GameObject thisMonster;
    public Animator anim;
    public GameManager gm;
    public int currentLevel;
    public MonsterInventory monsterInventory;

    [Header("Current Stats")]
    public int HPStat;
    public int AttackStat;
    public int MagicStat;
    public int DefenceStat;
    public int WillpowerStat;
    public int IntelligenceStat;
    public int WisdomStat;
    public float SpeedStat;

    public List<GenericTech> ownedTechs = new List<GenericTech>();

    void Awake()
    {
        //TODO refill monster inventory when scene changes
        if (this == GameObject.FindWithTag("Follower") && thisMonster == null)
        {
            if (monsterInventory.currentMonster == null)
            {
                thisMonster = monsterInventory.monsters[0];
            }
            else
            {
                thisMonster = monsterInventory.currentMonster;
            }
        }       
    }

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        UpdateCurrentStats();
        gm.UpdateHP();
        gm.UpdateEnergy();

        //if  (this == GameObject.FindWithTag("Follower") && thisMonster == null)
        //{
        //    thisMonster = gm.ownedMonsters.monsters[0];
        //}
    

        //anim = GetComponentInParent<Animator>();
        //anim.runtimeAnimatorController = thisMonster.GetComponent<ThisMonster>().thisMonster.monsterAnimator;

    }

 

    public void UpdateCurrentStats()
    {
        if (this.CompareTag("Follower"))
        {
            thisMonster = monsterInventory.currentMonster;
            anim = GetComponentInParent<Animator>();
            anim.runtimeAnimatorController = thisMonster.GetComponent<ThisMonster>().thisMonster.monsterAnimator;
            currentLevel = thisMonster.GetComponent<ThisMonster>().thisMonsterStats.level;
            gm.UpdateHP();
            gm.UpdateEnergy();
        } else
        {
            //need to change to area level
            currentLevel = 3;
            anim = GetComponentInParent<Animator>();
            anim.runtimeAnimatorController = thisMonster.GetComponent<ThisMonster>().thisMonster.monsterAnimator;
        }

        if (currentLevel == 0)
        {
            currentLevel = 1;
        }

        HPStat = ((3 * currentLevel) / 100 + currentLevel + 10);
        AttackStat = ((thisMonster.GetComponent<ThisMonster>().thisMonster.monsterStats.StrengthStat * 2) * currentLevel / 100) + 5;
        DefenceStat = ((thisMonster.GetComponent<ThisMonster>().thisMonster.monsterStats.StaminaStat * 2) * currentLevel / 100) + 5;
        MagicStat = ((thisMonster.GetComponent<ThisMonster>().thisMonster.monsterStats.MagicStat * 2) * currentLevel / 100) + 5;
        WillpowerStat = ((thisMonster.GetComponent<ThisMonster>().thisMonster.monsterStats.WillpowerStat * 2) * currentLevel / 100) + 5;
        IntelligenceStat = ((thisMonster.GetComponent<ThisMonster>().thisMonster.monsterStats.MagicStat * 2) * currentLevel / 100) + 5;
        WisdomStat = ((thisMonster.GetComponent<ThisMonster>().thisMonster.monsterStats.MagicStat * 2) * currentLevel / 100) + 5;
        SpeedStat = thisMonster.GetComponent<ThisMonster>().thisMonster.monsterStats.SpeedStat;
    }
}
