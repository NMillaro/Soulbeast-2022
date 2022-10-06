using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureReaction : MonoBehaviour
{
    public Ray ray;
    public RaycastHit hit;
    public GameObject pausePanel;
    public GameObject inventoryPanel;
    public GameObject gm;
    public CaptureManager capture;
    public InventoryItem thisItem;


    public void Use(InventoryItem captureItem)
    {
        thisItem = captureItem;
        inventoryPanel = GameObject.FindWithTag("InventoryPanel");
        inventoryPanel.SetActive(false);
        gm = GameObject.FindWithTag("GameManager");
        gm.GetComponent<GameManager>().gameState = GameState.ItemUse;
        capture = gm.GetComponent<CaptureManager>();
        capture.Capture(captureItem);
    }
    
}

