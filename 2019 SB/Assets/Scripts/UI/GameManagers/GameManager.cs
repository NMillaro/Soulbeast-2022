using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum CharacterState
{
    idle,
    walk,
    interact,
    attack,
    combat,
    stagger,
    dash
}

public enum GameState
{
    normal,
    pause,
    ItemUse
}

public class GameManager : MonoBehaviour
{
    [Header("Game Variables")]
    public GameManager gm;
    public static GameManager instance = null;
    public GameObject player;
    public GameObject follower;
    public CharacterState currentState;
    //public FloatValue currentHealth;
    public VectorValue playerPosition;
    public Message updateCoins;
    public Message updateHealth;
    public Message updateEnergy;
    public FloatValue currentHP;
    public FloatValue hpBar;
    public FloatValue currentEnergy;
    public FloatValue energyBar;
    public IntValue currentLevel;
    public LevelSystem levelSystem;
    public MonsterInventory ownedMonsters;
    public List<GameObject> tempMonsters = new List<GameObject>();

    [Header("UI Panels")]
    public PauseManager pauseManager;

    [Header("Game State System")]
    public FloatValue GameSpeed;
    public GameState gameState;

    [Header("Scriptable Objects")]
    public List<ScriptableObject> objects = new List<ScriptableObject>();


    [Header("Camera Variables")]
    public Name lastActiveChar;
    public GameObject currentActiveChar;
    public CinemachineVirtualCamera followCam;

    [Header("Transition Variables")]
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;

    [Header("Sound Effects")]
    public AudioClip musicClip;
    private AudioSource audioSrc;
    private bool isPlaying;

    private void OnEnable()
    {
        //LoadScriptables();
        //LoadData();

        if (ownedMonsters.monsters.Count == 3)
        {
            if (ownedMonsters.monsters[2] == null)
            {
                ownedMonsters.monsters.RemoveAt(2);
            }
        }
        if (ownedMonsters.monsters.Count == 2)
        {
            if (ownedMonsters.monsters[1] == null)
            {
                ownedMonsters.monsters.RemoveAt(1);
            }
        }
    }

    private void OnDisable()
    {
        // SaveScriptables();
    }



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }

    private void Start()
    {
        GameSpeed.RuntimeValue = GameSpeed.initialValue;
        gameState = GameState.normal;

        Invoke("UpdateUI", 0.1f);
    }

    public void UpdateHP()
    {
        currentHP.initialValue = follower.GetComponent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonsterStats.currentHP;
        if (currentHP.RuntimeValue > currentHP.initialValue)
        {
            currentHP.RuntimeValue = currentHP.initialValue;
        }
        hpBar.initialValue = currentHP.initialValue;
    }

    public void UpdateEnergy()
    {
        currentEnergy.initialValue = (follower.GetComponent<MonsterStats>().IntelligenceStat * 2) * 10;
        if (currentEnergy.RuntimeValue > currentEnergy.initialValue)
        {
            currentEnergy.RuntimeValue = currentEnergy.initialValue;
        }
        energyBar.initialValue = currentEnergy.initialValue;
    }

    void OnSceneLoaded(Scene previousScene, Scene newScene)
    {
        player = GameObject.FindWithTag("Player");
        follower = GameObject.FindWithTag("Follower");
        gm = this;

        LoadData();

        if (player != null)
        {
            player.transform.position = playerPosition.initialValue;
        }
        if (follower != null)
        {
            follower.transform.position = playerPosition.initialValue;
        }

        if (gm != null)
        {
            Invoke("UpdateUI", 0.1f);

        }

    }

    public void UpdatePosition()
    {
        playerPosition.initialValue = player.transform.position;
    }

    public void UpdateUI()
    {
        audioSrc = transform.Find("BGMusic").GetComponent<AudioSource>();

        if (!isPlaying)
        {
            audioSrc.Play();
            isPlaying = true;
        }
        currentActiveChar = GameObject.FindWithTag(lastActiveChar.initialName);
        levelSystem = GameObject.FindWithTag("LevelSystem").GetComponent<LevelSystem>();
        updateCoins.Raise();
        GetComponent<CaptureManager>().pauseManager = GameObject.FindWithTag("PauseCanvas").GetComponent<PauseManager>();

        if (follower != null)
        {
            UpdateHP();
            currentHP.RuntimeValue = currentHP.initialValue;
            updateHealth.Raise();
            UpdateEnergy();
            currentEnergy.RuntimeValue = currentEnergy.initialValue;
            updateEnergy.Raise();
        }
    }

    public void UpdateCam()
    {
        followCam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
        if (currentActiveChar != null)
        {
            followCam.Follow = currentActiveChar.transform;
        }
    }

    private void Update()
    {
        Invoke("UpdateCam", 0.3f);

        if (lastActiveChar.initialName == "Follower")
        {
            currentActiveChar = follower;
        }

        if (currentActiveChar != null)
        {
            if (currentActiveChar == player)
            {
                currentState = currentActiveChar.GetComponent<PlayerMain>().currentState;
            }
            else if (currentActiveChar == follower)
            {
                currentState = currentActiveChar.GetComponent<MonsterMain>().currentState;
            }
        }

        if (player.GetComponent<PlayerMain>().currentState != CharacterState.combat)
        {
            if (Input.GetKeyDown(KeyCode.E) && (currentState == CharacterState.walk || currentState == CharacterState.idle))
            {
                Swap(lastActiveChar.initialName);
            }
        }

        if (gameState == GameState.normal)
        {
            GameSpeed.RuntimeValue = 1.0f;
        }
    }

    public void Swap(string Name)
    {
        if (follower != null && GameSpeed.RuntimeValue != 0)
        {
            if (Name == "Player") //when E pressed swap PC between player/follower
            {
                lastActiveChar.initialName = "Follower";
                currentActiveChar = follower;



            }
            else if (Name == "Follower")
            {
                lastActiveChar.initialName = "Player";
                currentActiveChar = player;
            }
        }
    }

    public IEnumerator FadeCo()
    {
        if (fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(fadeWait);
        yield return null;
    }

    public void SaveData()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        //TODO monster clones persisting across scenes, memory leak    
        //if (tempMonsters.Count != 0)
        //{
        //    for (int i = 0; i <= tempMonsters.Count - 1; i++)
        //    {
        //        GameObject gameObjectToRemove = tempMonsters[i];
        //        Debug.Log("Destroying: " + gameObjectToRemove);
        //        Destroy(gameObjectToRemove);

        //    }
        //}
        //tempMonsters.Clear();

        ownedMonsters.monsters.Clear();

        if (data != null)
        {
            for (int i = 0; i < data.OwnedMonsters.Count; i++)
            {
                GameObject monster = Instantiate(GameAssets.i.Monsters[((int)data.OwnedMonsters[i]["number"]) - 1]);
                monster.GetComponent<ThisMonster>().thisMonsterStats.level = (int)data.OwnedMonsters[i]["level"];
                monster.GetComponent<ThisMonster>().thisMonsterStats.exp = (int)data.OwnedMonsters[i]["exp"];
                ownedMonsters.monsters.Add(monster);
                tempMonsters.Add(monster);

                // Debug.Log("Monster added to inventory: " + monster.GetComponent<ThisMonster>().thisMonster.monsterName);
            }
            ownedMonsters.currentMonster = ownedMonsters.monsters[(int)data.currentMonster];           
            ownedMonsters.currentMonster.GetComponent<ThisMonster>().thisMonsterStats.currentHP = (int)currentHP.RuntimeValue;
            follower.GetComponent<MonsterStats>().thisMonster = ownedMonsters.currentMonster;
            //ownedMonsters.currentMonster.GetComponent<ThisMonster>().thisMonsterStats.level = data.level;

        }
    }

    //public void SaveScriptables()
    //{
    //    for (int i = 0; i < objects.Count; i++)
    //    {
    //        FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}.dat", i));
    //        BinaryFormatter binary = new BinaryFormatter();
    //        var json = JsonUtility.ToJson(objects[i]);
    //        binary.Serialize(file, json);
    //        file.Close();
    //    }
    //}

    //public void LoadScriptables()
    //{
    //    for (int i = 0; i < objects.Count; i++)
    //    {
    //        if (File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
    //        {
    //            FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}.dat", i), FileMode.Open);
    //            BinaryFormatter binary = new BinaryFormatter();
    //            JsonUtility.FromJsonOverwrite((string)binary.Deserialize(file), objects[i]);
    //            file.Close();
    //        }
    //    }
    //}

    public void ResetScriptables()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
            {
                File.Delete(Application.persistentDataPath + string.Format("/{0}.dat", i));
            }
        }
        //Adds basic mon so inventory is not empty
        if (ownedMonsters.monsters[0] == null)
        {
            GameObject monster = Instantiate(GameAssets.i.Monsters[0]);
            ownedMonsters.monsters[0] = monster;
            ownedMonsters.currentMonster = monster;
            if (ownedMonsters.monsters.Count == 3)
            {
                if (ownedMonsters.monsters[2] == null)
                {
                    ownedMonsters.monsters.RemoveAt(2);
                }
            }
            if (ownedMonsters.monsters.Count == 2)
            {
                if (ownedMonsters.monsters[1] == null)
                {
                    ownedMonsters.monsters.RemoveAt(1);
                }
            }
        }
    }



}
