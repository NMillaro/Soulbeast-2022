using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterManager : UICanvasBase
{
    [Header("Inventory Info")]
    public MonsterInventory monsterInventory;
    public PauseManager pauseManager;
    [SerializeField] private GameObject blankInventorySlot = null;
    [SerializeField] private GameObject monsterInvImagePanel = null;
    [SerializeField] private GameObject monsterInvTextPanel = null;
    [SerializeField] private GameObject blankDescriptionText = null;
    [SerializeField] private GameObject useButton = null;
    public GameObject currentMonster;
    public GameObject selectedMonster;
    public Message FollowerSwappedMessage;

    public void Update()
    {
        if (Input.GetButtonDown("interact1") && dialogueBox.activeInHierarchy)
        {
            if (!textTyping)
            {
                dialogueBox.SetActive(false);
            }
        }
    }

    public void SetTextAndButton(bool buttonActive)
    {
        //descriptionText.GetComponent<TextMeshProUGUI>().text = description;
        if (buttonActive)
        {
            useButton.SetActive(true);
        }
        else
        {
            useButton.SetActive(false);
        }

    }

    void MakeInventorySlots()
    {
        if (monsterInventory)
        {
            for (int i = 0; i < monsterInventory.monsters.Count; i++)
            {
                if (monsterInventory.monsters[i])
                {
                    GameObject temp =
                       Instantiate(blankInventorySlot, monsterInvImagePanel.transform.position, Quaternion.identity);
                    temp.transform.SetParent(monsterInvImagePanel.transform);
                    MonsterSlot newSlot = temp.GetComponent<MonsterSlot>();
                    if (newSlot)
                    {
                        newSlot.Setup(monsterInventory.monsters[i], this);
                    }



                }
            }

            for (int i = 0; i < monsterInventory.monsters.Count; i++)
            {
                GameObject temp =
                    Instantiate(blankDescriptionText, monsterInvTextPanel.transform.position, Quaternion.identity);
                temp.transform.SetParent(monsterInvTextPanel.transform);
                TextMeshProUGUI monsterInfo = temp.GetComponent<TextMeshProUGUI>();
                if (monsterInfo)
                {
                    if (monsterInventory.monsters[i].GetComponent<ThisMonster>().thisMonsterStats.nickname == "")
                    {
                        monsterInfo.text = (monsterInventory.monsters[i].GetComponent<ThisMonster>().thisMonster.monsterName + "\nHP: " +
                            monsterInventory.monsters[i].GetComponent<ThisMonster>().thisMonsterStats.currentHP) + "/" +
                            ((3 * monsterInventory.monsters[i].GetComponent<ThisMonster>().thisMonsterStats.level) / 100 + monsterInventory.monsters[i].GetComponent<ThisMonster>().thisMonsterStats.level + 10)
                            + "\nLvl " + monsterInventory.monsters[i].GetComponent<ThisMonster>().thisMonsterStats.level;
                    }
                    else
                    {
                        monsterInfo.text = monsterInventory.monsters[i].GetComponent<ThisMonster>().thisMonsterStats.nickname;
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        ClearInventorySlots();
        MakeInventorySlots();
        SetTextAndButton(false);
        currentMonster = monsterInventory.currentMonster;
        pauseManager = GameObject.FindWithTag("PauseCanvas").GetComponent<PauseManager>();
    }

    // Update is called once per frame
    public void ActivateButton(GameObject newMonster)
    {
        useButton.SetActive(true);
    }

    void ClearInventorySlots()
    {
        for (int i = 0; i < monsterInvImagePanel.transform.childCount; i++)
        {
            Destroy(monsterInvImagePanel.transform.GetChild(i).gameObject);
            Destroy(monsterInvTextPanel.transform.GetChild(i).gameObject);
        }
    }

    public void UseButtonPressed()
    {
        if (currentMonster != selectedMonster)
        {            
            monsterInventory.currentMonster = selectedMonster;
            FollowerSwappedMessage.Raise(); //follower, levelsystem, leveltextmanager scripts update monster
            Debug.Log("Monster swapped");
            pauseManager.ChangePause(); 
        }

        else
        {
            if (!textTyping)
            {

                dialogueBox.SetActive(true);
                if (currentMonster.GetComponent<ThisMonster>().thisMonsterStats.nickname == "")
                {
                    dialogue = currentMonster.GetComponent<ThisMonster>().thisMonster.monsterName + " is already out!";
                }
                else
                {
                    dialogue = currentMonster.GetComponent<ThisMonster>().thisMonsterStats.nickname + " is already out!";
                }
                StartCoroutine(ShowText());
            }
        }

        
    }
}
