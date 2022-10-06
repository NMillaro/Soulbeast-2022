using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public GameObject player;
    public Name lastActiveChar;
    public CharacterState currentState;
    private GameObject gameManager;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy") && !other.isTrigger)
        {
            gameObject.GetComponentInParent<PlayerMain>().currentState = CharacterState.combat;
            gameManager.GetComponent<GameManager>().Swap("Player");

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy") && !other.isTrigger)
        {
            gameObject.GetComponentInParent<PlayerMain>().currentState = CharacterState.idle;
        }
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameManager = GameObject.FindWithTag("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
