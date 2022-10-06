using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Info")]
    public PlayerInventory playerInventory;
    [SerializeField] private GameObject blankInventorySlot = null;
    [SerializeField] private GameObject inventoryPanel = null;
    [SerializeField] private TextMeshProUGUI descriptionText = null;
    [SerializeField] private GameObject useButton = null;
    public InventoryItem currentItem;

    public void SetTextAndButton(string description, bool buttonActive)
    {
        descriptionText.text = description;
        if (buttonActive)
        {
            useButton.SetActive(true);
        } else
        {
            useButton.SetActive(false);
        }

    }

    void MakeInventorySlots()
    {
        if (playerInventory)
        {
            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {
                if (playerInventory.myInventory[i].numberHeld > 0)
                {
                    GameObject temp =
                       Instantiate(blankInventorySlot, inventoryPanel.transform.position, Quaternion.identity);
                    temp.transform.SetParent(inventoryPanel.transform);
                    InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                    if (newSlot)
                    {
                        newSlot.Setup(playerInventory.myInventory[i], this);
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        ClearInventorySlots();
        MakeInventorySlots();
        SetTextAndButton("", false);
    }

    // Update is called once per frame
    public void SetupDescAndButton(string newDescString, bool isButtonUsable, InventoryItem newItem)
    {
        currentItem = newItem;
        descriptionText.text = newDescString;
        useButton.SetActive(isButtonUsable);
    }

    void ClearInventorySlots()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }

    public void UseButtonPressed()
    {
        if (currentItem)
        {
            currentItem.Use();
            //Clear inventory slots
            ClearInventorySlots();
            //Refill all slots
            MakeInventorySlots();

            if (currentItem.numberHeld == 0)
            {
                SetTextAndButton("", false);
                playerInventory.myInventory.Remove(currentItem);
            }
        }
    }
}
