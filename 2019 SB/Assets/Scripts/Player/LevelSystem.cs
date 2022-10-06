using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int exp;
    [SerializeField] private int expToNextLevel;
    [SerializeField] private int expGroup;
    [SerializeField] public Message expMessage;
    [SerializeField] public Message lvlUpMessage;
    [SerializeField] private LevelTextManager lvlTxtMan = null;

    [Header("Particle Effect Variables")]
    public GameObject particleEffect;
    public float effectDelay;

    [Header("LevelUp Effect Variables")]
    [SerializeField] private float fadeTimer = 0;
    [SerializeField] private float scaleAmount = 0;
    [SerializeField] private bool isLevelingUp = false;

    //public Color fadeColor;
    //public Color regularColor;
    //public float fadeInTime;
    //public float fadeOutTime;
    //public float fadeDelay;
    //public SpriteRenderer mySprite;

    private void Start()
    {
        //TODO Add message raise to check for when monster swapped from inventory
        UpdateAll();
    }

    private void OnEnable()
    {
        Invoke("UpdateAll", 0.1f);
        //UpdateAll();
    }

    public int FetchExpGroup()
    {
        int temp = GetComponentInParent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonster.expGroup;
        return temp;

    }


    public void AddExperience(int amount)
    {
        exp += amount;
        GetComponentInParent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonsterStats.exp = exp;
        lvlTxtMan.isAnimating = true;
        while (exp >= expToNextLevel)
        {
            LevelUp();
        }
        if (expMessage != null)
        {
            expMessage.Raise();
        }
    }

    public int GetLevelNumber()
    {
        return level;
    }

    public float GetExpNormalized()
    {
        return (float)exp / expToNextLevel;
    }

    public int GetExperience()
    {
        return exp;
    }

    public int GetExpToNextLevel()
    {
        return expToNextLevel;
    }

    public void LevelUp()
    {
        exp -= expToNextLevel;
        level++;
        GetComponentInParent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonsterStats.level = level;
        lvlUpMessage.Raise();
        StartCoroutine(ScaleCo());
        SpawnParticleEffect(particleEffect, effectDelay);
        GetComponentInParent<MonsterStats>().UpdateCurrentStats();
    }

    public void UpdateExpToNextLevel()
    {
        expToNextLevel = MonsterData.ExpGroupData[expGroup][level];
    }

    public void SpawnParticleEffect(GameObject particleEffect, float EffectDelay)
    {
        if (particleEffect != null)
        {
            GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.identity);
            Destroy(effect, EffectDelay);
        }
    }

    public void UpdateAll()
    {
        level = GetComponentInParent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonsterStats.level;
        if (level == 0)
        {
            level = 1;
        }
        expGroup = FetchExpGroup();
        exp = GetComponentInParent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonsterStats.exp;
        //mySprite = GetComponentInParent<SpriteRenderer>();
        //regularColor = GetComponentInParent<SpriteRenderer>().color;
        UpdateExpToNextLevel();
    }


    private IEnumerator ScaleCo() //makes Monster bigger for faderTimer seconds while leveling up
    {
        isLevelingUp = true;
        float temp = fadeTimer;
        while (isLevelingUp)
        {
            temp -= Time.deltaTime;
            if (temp > fadeTimer * .5f)
            {
                transform.parent.localScale += Vector3.one * scaleAmount * Time.deltaTime;
            }
            else
            {
                transform.parent.localScale -= Vector3.one * scaleAmount * Time.deltaTime;
            }

            if (temp <= 0)
            {
                isLevelingUp = false;
            }
            yield return null;
        }
    }

    //public void CallFadeCo()
    //{
    //    StartCoroutine(FadeInCo());
    //}


    //private IEnumerator FadeInCo()
    //{
    //    for (float t = 0.01f; t < fadeInTime; t += 0.1f)
    //    {
    //        regularColor = Color.Lerp(regularColor, fadeColor, t / fadeInTime);
    //        yield return null;
    //    }
    //    StartCoroutine(FadeOutCo());
    //}

    //private IEnumerator FadeOutCo()
    //{
    //    for (float t = 0.01f; t < fadeOutTime; t += 0.1f)
    //    {
    //        regularColor = Color.Lerp(fadeColor, regularColor, t / fadeOutTime);
    //        yield return null;
    //    }

    //}

}
