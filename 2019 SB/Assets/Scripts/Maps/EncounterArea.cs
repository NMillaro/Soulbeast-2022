using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterArea : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyEncounterPrefab;

    public BiomeList biomeType;
    public GameObject player;
    private bool spawning = false;
    public bool playerInRange = false;
    //private Vector3 oldPosition;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && playerInRange && player != null)
        {   
            float vc = 10f / 187.5f;
            //float c = 8.5f / 187.5f;
            //float uc = 6.75f / 187.5f;
            //float r = 3.33f / 187.5f;
            //float vr = 1.25f / 187.5f;

            float p = Random.Range(0.0f, 100.0f);
            Debug.Log("p = " + p);

            if (p < vc * 100)
            {
                player.GetComponent<PlayerMain>().currentState = CharacterState.attack;
                this.spawning = true;
                SceneManager.LoadScene("Battle");

            }

            //StartCoroutine(wait());
        
        }


    }

    //private IEnumerator wait()
    //{
    //    yield return new WaitForSeconds(5);
    //}

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle")
        {
            if (this.spawning)
            {
                //Instantiate(enemyEncounterPrefab);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
         
        }
    }


}
