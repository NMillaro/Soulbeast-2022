using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh = null;
    [SerializeField] private Slider playerExpFill = null;
    private GameObject currentMonster;
    [SerializeField] private LevelSystem levelSystem = null;
    [SerializeField] public bool isAnimating = false;
    [SerializeField] private int level = 0;
    [SerializeField] private int exp = 0;
    [SerializeField] private int expToNextLevel = 0;

    [Header("ExpBar")]
    private float updateTimer;
    [SerializeField] private float updateTimerMax = 0.016f;


    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Invoke("SystemStart", 0.1f);

    }

    private void SystemStart()
    {
        levelSystem = GameObject.FindWithTag("LevelSystem").GetComponent<LevelSystem>();
        SetLevelSystem(levelSystem);
    }

    private void Update()
    {
        if (isAnimating)
        {
            textMesh.SetText("Lvl " + level);
            // updateTimerMax = ((float)exp + 1f  / expToNextLevel)/50;
            updateTimer += Time.deltaTime;
            while (updateTimer > updateTimerMax)
            {
                updateTimer -= updateTimerMax;
                UpdateExpBar();
            }
        }      
    }

    private void UpdateExpBar()
    {
        if (level < levelSystem.GetLevelNumber())
        {
            AddExperience();
            SetExpBarSize((float)exp / expToNextLevel);
        }
        else
        {
            if (exp < levelSystem.GetExperience())
            {
                AddExperience();
                SetExpBarSize((float)exp / expToNextLevel);
            }
            else
            {
                isAnimating = false;
            }
        }
        //Debug.Log(level + " " + exp);
    }

    private void AddExperience()
    {
        exp++;
        if (exp >= expToNextLevel)
        {
            level++;
            exp = 0;
        }
    }

    public void UpdateUI()
    {
        isAnimating = true;
        textMesh.SetText("Lvl " + level);
    }

    private void SetExpBarSize(float ExpNormalized)
    {
        playerExpFill.value = ExpNormalized;
    }

    public void SetLevelNumber(int levelNumber)
    {
        textMesh.SetText("Lvl " + level);
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;

        level = levelSystem.GetLevelNumber();
        exp = levelSystem.GetExperience();
        expToNextLevel = levelSystem.GetExpToNextLevel();
        SetLevelNumber(levelSystem.GetLevelNumber());
        SetExpBarSize(levelSystem.GetExpNormalized());
    }

    public void MonsterSwappedUpdateUI()
    {
        Invoke("SwapUI", 0.01f);
        
    }

    public void SwapUI()
    {
        level = levelSystem.GetLevelNumber();
        exp = levelSystem.GetExperience();
        expToNextLevel = levelSystem.GetExpToNextLevel();
        SetLevelNumber(levelSystem.GetLevelNumber());
        SetExpBarSize(levelSystem.GetExpNormalized());

        textMesh.SetText("Lvl " + level);
    }
}
