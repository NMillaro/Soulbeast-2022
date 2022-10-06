using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureManager : MonoBehaviour
{
    public GameObject gm;
    public InventoryItem thisItem;
    public GameObject hover;
    public PauseManager pauseManager;
    private Hover hoverScript;


    void OnEnable()
    {
        gm = GameObject.FindWithTag("GameManager");
        pauseManager = GameObject.FindWithTag("PauseCanvas").GetComponent<PauseManager>();
        hover = GameObject.FindWithTag("Hover");
        hoverScript = hover.GetComponent<Hover>();
    }


    void Update()
    {
        if (gm.GetComponent<GameManager>().gameState == GameState.ItemUse)
        {
            if (thisItem && Input.GetButtonDown("interact1"))
            {
                if (hoverScript.currentSelection)
                {
                    if (hoverScript.currentSelection.GetComponentInChildren<CaptureAttempt>().isCapturing == false)
                    {
                        hoverScript.currentSelection.GetComponentInChildren<CaptureAttempt>().Capturing();
                        thisItem.DecreaseAmount(1);
                    }

                    gm.GetComponent<GameManager>().gameState = GameState.normal;
                    hoverScript.spriteRenderer.sprite = null;
                    pauseManager.ChangePause();
                }
            }

        }
    }

    public void Capture(InventoryItem CaptureItem)
    {
        thisItem = CaptureItem;
        hoverScript.Activate(thisItem);


            //hover.GetComponent<Hover>().
           // GameObject newHover = Instantiate(hover, mousePosition,
           //Quaternion.Euler(0f, 0f, 0f));
        

    }

}
