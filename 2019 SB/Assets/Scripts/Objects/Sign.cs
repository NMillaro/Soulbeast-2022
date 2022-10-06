using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable {

    //private Animator animator;
    public Text dialogueText;
    public string dialogue;
    public bool playerUp;
    public Vector3 playerPosition;
    public Vector3 objectPosition;
    public Vector3 faceObjectDirection;
    public GameObject player;
    Camera pCamera;

    private AudioSource audioSrc;
    public AudioClip typeClip;

    protected override void Start()
    {
        base.Start();

        pCamera = Camera.main.GetComponent<Camera>();
        player = GameObject.FindWithTag("Player");
        audioSrc = GameObject.Find("SpeechSound").GetComponent<AudioSource>();
    }

    IEnumerator ShowText()
    {
        textTyping = true;

        for (int i = 0; i <= dialogue.Length; i++)
        {
            currentText = dialogue.Substring(0, i);
            dialogueText.text = currentText;
            audioSrc.PlayOneShot(typeClip);
            yield return new WaitForSeconds(delay.initialValue);
        }

        yield return new WaitForSeconds(0.25f); //delay after text finished
        textTyping = false;
    }

    protected virtual void Update () {

        playerPosition = pCamera.WorldToScreenPoint(player.transform.position);
        objectPosition = pCamera.WorldToScreenPoint(transform.position);

        faceObjectDirection =  objectPosition - playerPosition;

        currentActiveChar = gameManager.GetComponent<GameManager>().currentActiveChar;


        if (Input.GetButtonDown("interact1") && playerInRange && currentActiveChar == GameObject.FindWithTag("Player"))
        {
            if (dialogueBox.activeInHierarchy && !textTyping)
            {
                dialogueBox.SetActive(false);
                currentActiveChar.GetComponent<PlayerMain>().currentState = CharacterState.walk;
                if (minimap != null)
                {
                    minimap.SetActive(true);
                }
            }
            else
            {
                if (!textTyping)
                {
                    if (minimap != null)
                    {
                        minimap.SetActive(false);
                    }
                    currentActiveChar.GetComponent<PlayerMain>().currentState = CharacterState.interact;
                    currentActiveChar.GetComponent<Animator>().SetBool("moving", false);
                    ChangeAnim(faceObjectDirection);
                    dialogueBox.SetActive(true);

                    StartCoroutine(ShowText());
                }
                //else if (textTyping)
                //{
                //    StopCoroutine(ShowText());
                //    dialogueText.text = dialogue;
                //    textTyping = false;
                //}
            }
        }     	
	}

    private void SetAnimFloat(Vector2 setVector)
    {
        currentActiveChar.GetComponent<Animator>().SetFloat("moveX", setVector.x);
        currentActiveChar.GetComponent<Animator>().SetFloat("moveY", setVector.y);
    }

    private void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);

            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }
    }


}
