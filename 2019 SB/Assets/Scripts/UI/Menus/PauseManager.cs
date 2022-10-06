using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance = null;
    private bool isPaused;
    public GameObject pausePanel;
    public GameObject UIPanel; //player health, energy, minimap etc.
    public GameObject inventoryPanel;
    public GameObject monstersPanel;
    public bool usingPausePanel;
    public GameObject gm;
    public string mainMenu;
    public FloatValue gameSpeed;
    public GameObject hover;
    private Hover hoverScript;

    // Start is called before the first frame update
    void OnEnable()
    {
        isPaused = false;
        usingPausePanel = false;
        pausePanel.SetActive(false);
        inventoryPanel.SetActive(false);
        monstersPanel.SetActive(false);
        gm = GameObject.FindWithTag("GameManager");
        gameSpeed = gm.GetComponent<GameManager>().GameSpeed;
        hover = GameObject.FindWithTag("Hover");
        hoverScript = hover.GetComponent<Hover>();
        UIPanel = GameObject.FindWithTag("UICanvas");
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("pause"))
        {
            ChangePause();
        }       
    }

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else if (instance != this)
    //    {
    //        Destroy(gameObject);
    //    }
    //    DontDestroyOnLoad(gameObject);
    //    SceneManager.activeSceneChanged += OnSceneLoaded;
    //}

    //void OnSceneLoaded(Scene previousScene, Scene newScene)
    //{
    //    instance = this;
    //    UIPanel = GameObject.FindWithTag("UICanvas");
    //    usingPausePanel = false;
    //}

    public void ChangePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            UIPanel.GetComponent<Canvas>().enabled = false;
            pausePanel.SetActive(true);
            gameSpeed.RuntimeValue = 0f;
            usingPausePanel = true;
            gm.GetComponent<GameManager>().gameState = GameState.pause;
        }
        else
        {
            inventoryPanel.SetActive(false);
            pausePanel.SetActive(false);
            monstersPanel.SetActive(false);
            UIPanel.GetComponent<Canvas>().enabled = true;
            gameSpeed.RuntimeValue = 1f;
            hoverScript.spriteRenderer.sprite = null;
            if (hoverScript.currentSelection != null)
            {
                hoverScript.currentSelection.GetComponent<ContextClue>().ContextOff();
                hoverScript.currentSelection = null;
            }
            hoverScript.isSelected = false;
            gm.GetComponent<GameManager>().gameState = GameState.normal;
            gm.GetComponent<CaptureManager>().thisItem = null;
            usingPausePanel = false;
        }

    }

    public void QuitToMainMenu()
    {
        Destroy(GameObject.FindWithTag("GameManager"));
        Destroy(GameObject.FindWithTag("Hover"));
        SceneManager.LoadScene(mainMenu);

    }

    public void SwitchPanels(string panel)
    {
        usingPausePanel = !usingPausePanel;
        if (panel == "pause")
        {
            pausePanel.SetActive(true);
            inventoryPanel.SetActive(false);
            monstersPanel.SetActive(false);
        }
        if (panel == "inventory")
        {
            inventoryPanel.SetActive(true);
            pausePanel.SetActive(false);
            monstersPanel.SetActive(false);
        }
        if (panel == "soulbeasts")
        {
            inventoryPanel.SetActive(false);
            pausePanel.SetActive(false);
            monstersPanel.SetActive(true);
        }

    }

}
