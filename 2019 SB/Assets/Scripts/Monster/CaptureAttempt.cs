using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureAttempt : MonoBehaviour
{
    public GameObject gm;
    public bool isCapturing = false;
    
    void OnEnable()
    {
        gm = GameObject.FindWithTag("GameManager");
    }

    private void Update()
    {
        if (isCapturing)
        {
            gameObject.transform.parent.GetComponentInChildren<ContextClue>().ContextOn();

            //if (gameObject.transform.parent.GetComponentInChildren<BasicEnemyHealth>().currentHealth <= 0)
            //{
            //    gm.GetComponent<GameManager>().ownedMonsters.AddMonster
            //        (gameObject.GetComponentInParent<MonsterStats>().thisMonster);
            //}
        }
        //else
        //{
        //    gameObject.transform.parent.GetComponentInChildren<ContextClue>().ContextOff();
        //}
        
    }

    // Update is called once per frame
    public void Capturing()
    {
        isCapturing = true;

    }
}
