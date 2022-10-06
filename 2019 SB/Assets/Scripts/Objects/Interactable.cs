using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Message context;
    public bool playerInRange;
    public GameObject currentActiveChar;
    public GameObject gameManager;
    public GameObject minimap;
    public GameObject dialogueBox;
    protected string currentText;
    protected bool textTyping = false;
    [SerializeField] protected FloatValue delay;


    protected virtual void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        minimap = GameObject.FindWithTag("Minimap");
    }

    protected virtual void OnEnable()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        minimap = GameObject.FindWithTag("Minimap");
    }


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (context != null){
                context.Raise();
            }
            playerInRange = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (context != null)
            {
                context.Raise();
            }
            playerInRange = false;
        }
    }



}
