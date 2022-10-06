using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSlot : MonoBehaviour
{
    [Header("UI Variables")]
    //Name/Health etc. in separate scroll area next to image
    //[SerializeField] private TextMeshProUGUI monsterName = null; 
    [SerializeField] private Image monsterImage = null;

    [Header("Item Variables")]
    public GameObject thisMonster;
    public MonsterManager thisManager;

    public void Setup(GameObject newMonster, MonsterManager newManager)
    {
        thisMonster = newMonster;
        thisManager = newManager;

        if (thisMonster)
        {
            monsterImage.sprite = thisMonster.GetComponent<ThisMonster>().thisMonster.monsterImage;
            //if (thisMonster.GetComponent<ThisMonster>().thisMonsterStats.nickname == "")
            //{
            //    monsterName.text = "" + thisMonster.GetComponent<ThisMonster>().thisMonster.monsterName;
            //}
            //else
            //{
            //    monsterName.text = "" + thisMonster.GetComponent<ThisMonster>().thisMonsterStats.nickname;
            //}
        }

    }

    public void ClickedOn()
    {
        Debug.Log("Clicked on " + thisMonster.GetComponent<ThisMonster>().thisMonster.monsterName);
        thisManager.ActivateButton(thisMonster);
        thisManager.selectedMonster = thisMonster;
        //else
        //{
        //    thisManager.SetupDescAndButton(thisMonster.GetComponent<ThisMonster>().thisMonsterStats.nickname, thisMonster.GetComponent<MonsterStats>().thisMonster);
        //}
    }
}
