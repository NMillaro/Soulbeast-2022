using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMove : MonoBehaviour {

    public Vector3 playerChange;
    public bool needText;
    public string placeName;
    public GameObject text;
    public Text placeText;
    private GameObject player;
    private GameObject follower;
    private GameObject currentActiveChar;
    public Name lastActiveChar;

    void Start ()
    {
        player = GameObject.FindWithTag("Player");
        follower = GameObject.FindWithTag("Follower");
    }
	
	void Update ()
    {
        currentActiveChar = GameObject.FindWithTag(lastActiveChar.initialName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject == player || other.gameObject == follower) && !other.isTrigger)
        {
            if (currentActiveChar == player)
            {
                player.transform.position += playerChange;
                follower.transform.position = player.transform.position;
            }
            else if (currentActiveChar == follower)
            {
                follower.transform.position += playerChange;
                player.transform.position = follower.transform.position;
            }

            if (needText)
            {
                StartCoroutine(placeNameCo());
            }
        }  
    }

    private IEnumerator placeNameCo()
    {
        text.SetActive(true);
        placeText.text = placeName;
        yield return new WaitForSeconds(4f);
        text.SetActive(false);
    }
}
