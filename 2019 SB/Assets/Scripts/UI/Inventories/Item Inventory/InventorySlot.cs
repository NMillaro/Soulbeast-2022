using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI Variables")]
    [SerializeField] private TextMeshProUGUI itemNumberText = null;
    [SerializeField] private Image itemImage = null;

    [Header("Item Variables")]
    public InventoryItem thisItem;
    public InventoryManager thisManager;

    public void Setup(InventoryItem newItem, InventoryManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;

        if (thisItem)
        {
            itemImage.sprite = thisItem.itemImage;
            itemNumberText.text = "" + thisItem.numberHeld;
        }

    }

    public void ClickedOn()
    {
        thisManager.SetupDescAndButton(thisItem.itemDescription, thisItem.usable, thisItem);
    }
}
