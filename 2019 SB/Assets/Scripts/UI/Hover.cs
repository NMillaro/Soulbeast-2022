using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hover : MonoBehaviour
{
    public static Hover instance = null;
    public InventoryItem thisItem;
    public Transform contextClue;
    Camera pCamera;
    public SpriteRenderer spriteRenderer;
    public bool isSelected = false;
    public GameObject gm;
    public GameObject currentSelection;

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

    void OnSceneLoaded(Scene previousScene, Scene newScene)
    {
        pCamera = Camera.main.GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        pCamera = Camera.main.GetComponent<Camera>();
        gm = GameObject.FindWithTag("GameManager");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    

    private void FollowMouse()
    {
        transform.position = pCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void Activate(InventoryItem currentItem)
    {
        thisItem = currentItem;
        spriteRenderer.sprite = thisItem.itemImage;

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy" && other.isTrigger && gm.GetComponent<GameManager>().gameState == GameState.ItemUse)
        {
            if (!isSelected)
            {
                isSelected = true;
                currentSelection = other.transform.parent.gameObject;
                currentSelection.GetComponent<ContextClue>().ContextOn();
            }
        }
    }


    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "enemy" && other.transform.parent.gameObject == currentSelection && other.isTrigger 
            && gm.GetComponent<GameManager>().gameState == GameState.ItemUse)
        {
            currentSelection.GetComponentInChildren<TargetSelection>().CallFlashCo();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "enemy" && other.transform.parent.gameObject == currentSelection && other.isTrigger 
            && gm.GetComponent<GameManager>().gameState == GameState.ItemUse)
        {
            if (isSelected)
            {
                other.GetComponentInParent<ContextClue>().ContextOff();
                isSelected = false;
                currentSelection = null;
            }
        }
    }
}
