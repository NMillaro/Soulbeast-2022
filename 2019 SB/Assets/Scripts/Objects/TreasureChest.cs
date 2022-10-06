using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChest : Interactable
{
    [Header("Contents")]
    public Item contents;
    public Inventory playerInventory;
    public bool isOpen;
    public BoolValue storedOpen; 

    [Header("Signals and Dialogue")]
    public Message raiseItem;
    public Text dialogueText;

    [Header("Animation")]
    private Animator anim;


    IEnumerator ShowText()
    {
        textTyping = true;

        for (int i = 0; i <= contents.itemDescription.Length; i++)
        {
            currentText = contents.itemDescription.Substring(0, i);
            dialogueText.text = currentText;
            yield return new WaitForSeconds(delay.initialValue);
        }

        textTyping = false;
    }

    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        isOpen = storedOpen.RuntimeValue;
        if (isOpen)
        {
            anim.SetBool("opened", true);
        }
        
    }

    void Update()
    {
        currentActiveChar = gameManager.GetComponent<GameManager>().currentActiveChar;

        if (Input.GetButtonDown("interact1") && playerInRange && currentActiveChar == GameObject.FindWithTag("Player"))
        {
            if (dialogueBox.activeInHierarchy && !textTyping)
            {
                dialogueBox.SetActive(false);
                raiseItem.Raise();
                playerInRange = false;
               // currentActiveChar.GetComponent<PlayerMain>().currentState = PlayerState.walk;
                if (minimap != null)
                {
                    minimap.SetActive(true);
                }
            }
            else
            {
                if (!textTyping)
                {
                    // OpenChest();
                    if (minimap != null)
                    {
                        minimap.SetActive(false);
                    }
                    currentActiveChar.GetComponent<Animator>().SetBool("moving", false);
                    playerInventory.AddItem(contents);
                    playerInventory.currentItem = contents;
                    raiseItem.Raise();
                    context.Raise();
                    isOpen = true;
                    anim.SetBool("opened", true);
                    storedOpen.RuntimeValue = isOpen;

                    dialogueBox.SetActive(true);
                    StartCoroutine(ShowText());
                }
            } 
        }
    }

    //public void OpenChest()
    //{
    //    if (!textTyping)
    //    {
    //        if (minimap != null)
    //        {
    //            minimap.SetActive(false);
    //        }
    //        currentActiveChar.GetComponent<PlayerMain>().currentState = PlayerState.interact;
    //        currentActiveChar.GetComponent<Animator>().SetBool("moving", false);
    //        playerInventory.AddItem(contents);
    //        playerInventory.currentItem = contents;
    //        raiseItem.Raise();
    //        context.Raise();
    //        isOpen = true;
    //        anim.SetBool("opened", true);
    //        storedOpen.RuntimeValue = isOpen;

    //        dialogueBox.SetActive(true);
    //        StartCoroutine(ShowText());
    //    }       
    //}


    //public void ChestEmpty()
    //{
    //    if (dialogueBox.activeInHierarchy && !textTyping)
    //    {
    //        dialogueBox.SetActive(false);
    //        raiseItem.Raise();
    //        playerInRange = false;
    //        currentActiveChar.GetComponent<PlayerMain>().currentState = PlayerState.walk;
    //        if (minimap != null)
    //        {
    //            minimap.SetActive(true);
    //        }
    //    }
    //}

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = false;
        }
    }
}
