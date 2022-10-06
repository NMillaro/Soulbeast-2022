using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [Header("Player Variables")]
    public CharacterState currentState;
    public float speed; //movement and follow speed
    public float distance; //follows after x distance from player
    public float combatDistance; 
    private Rigidbody2D myRigidbody;
    protected Animator animator;
    protected Vector3 change;
    private bool isKnocked = false;


    [Header("Sound Effects")]
    private AudioSource audioSrc;
    public float pitchMin;
    public float pitchMax;
    public float volumeMin;
    public float volumeMax;
    private bool isPlaying = false;



    //TODO GM move to GM
    [Header("Game Variables")]
    public GameObject player;
    public GameObject follower;
    protected GameObject gameManager;
    private float gameSpeed;
    public GameObject currentActiveChar;
    public VectorValue currentPosition;
    public Vector3 monsterDirection;
    

    //TODO GM move to GM
    [Header("Camera Variables")] 
    public Vector3 mousePosition;
    public Vector3 playerPosition;
    private CameraMovement cam;
    Camera pCamera;

    //TODO break inventory into own component
    [Header("Inventory")]
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;

    void Start()
    {
        currentState = CharacterState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        follower = GameObject.FindWithTag("Follower");
        gameManager = GameObject.FindWithTag("GameManager");
        pCamera = Camera.main.GetComponent<Camera>();
        audioSrc = GetComponent<AudioSource>();


        if (Camera.main.GetComponent<CameraMovement>() != null)
        {
            cam = Camera.main.GetComponent<CameraMovement>();
        }

        //start facing down
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

    }


    void Update()
    {
        currentActiveChar = gameManager.GetComponent<GameManager>().currentActiveChar;
        mousePosition = Input.mousePosition;
        playerPosition = pCamera.WorldToScreenPoint(player.transform.position);
        gameSpeed = gameManager.GetComponent<GameManager>().GameSpeed.RuntimeValue;

        if (follower != null)
        {
            monsterDirection = pCamera.WorldToScreenPoint(follower.transform.position) - playerPosition;
        }

        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (!isKnocked)
        {
            myRigidbody.velocity = Vector2.zero;
        }

        if (currentState == CharacterState.interact)
        {
            return;
        }

        if (gameSpeed != 0)
        {
            animator.speed = 1f;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (currentState == CharacterState.combat)
            {

                if (Vector3.Distance(transform.position, currentActiveChar.transform.position) > combatDistance) // if distance from player target is greater than distance variable, move toward player
                {
                    Vector3 temp = Vector3.MoveTowards(transform.position, currentActiveChar.transform.position, speed * Time.deltaTime);

                    ChangeAnim(temp - transform.position);
                    animator.SetBool("moving", true);
                    transform.position = Vector2.MoveTowards(transform.position, currentActiveChar.transform.position, speed * Time.deltaTime); //move towards the player


                    if (Vector3.Distance(transform.position, currentActiveChar.transform.position) > 10) //if distance greater than 10, teleport to player
                    {
                        transform.position = (currentActiveChar.transform.position);
                    }
                }

                else if (change == Vector3.zero || Vector3.Distance(transform.position, currentActiveChar.transform.position) < combatDistance / 1.05) // if distance from player target is greater than distance variable, move toward player
                {
                    animator.SetBool("moving", false);
                    ChangeAnim(monsterDirection);

                }
            }

            if (currentActiveChar == player && (currentState == CharacterState.walk || currentState == CharacterState.idle))
            {
                UpdateAnimationAndMove();
            }

            if (currentActiveChar == follower && (currentState == CharacterState.walk || currentState == CharacterState.idle) && player.GetComponent<PlayerMain>().currentState != CharacterState.interact)
            {
                if (follower != null)
                {
                    MoveCharacter();
                }
            }
        }

        else if (gameSpeed == 0)
        {
            animator.speed = 0f;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }



    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            change.Normalize();
            myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
            currentState = CharacterState.walk;
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
            if (!isPlaying)
            {
                StartCoroutine(FootstepSound());
            }
        }
        else
        {
            currentState = CharacterState.idle;
            animator.SetBool("moving", false);
            
        }
    }

    void MoveCharacter()
    {
        Vector3 temp = Vector3.MoveTowards(transform.position, currentActiveChar.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentActiveChar.transform.position) > distance) // if distance from player target is greater than distance variable, move toward player
        {

            ChangeAnim(temp - transform.position);
            animator.SetBool("moving", true);
            transform.position = Vector2.MoveTowards(transform.position, currentActiveChar.transform.position, speed * Time.deltaTime); //move towards the player


            if (Vector3.Distance(transform.position, currentActiveChar.transform.position) > 10) //if distance greater than 10, teleport to player
            {
                transform.position = (currentActiveChar.transform.position);
            }
        }
        else if (change == Vector3.zero || (Vector3.Distance(transform.position, currentActiveChar.transform.position) < distance / 1.1))
        {
            animator.SetBool("moving", false);
        }
    }
  
    public void RaiseItem()
    {
        if (playerInventory.currentItem != null)
        {
            if (currentState != CharacterState.interact)
            {
                animator.SetBool("receiveItem", true);
                currentState = CharacterState.interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("receiveItem", false);
                currentState = CharacterState.idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
    }
    
    //TODO KNOCKBACK
    public void Knock(float knockTime)
    {
        if (!isKnocked)
        {
            StartCoroutine(KnockCo(knockTime));
        }
    }

    //TODO KNOCKBACK
    private IEnumerator KnockCo(float knockTime)
    {
        isKnocked = true;

        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
        }

        isKnocked = false;
    }

    //footstep sound effects
    private IEnumerator FootstepSound()
    {
        isPlaying = true;

        audioSrc.pitch = Random.Range(pitchMin, pitchMax);
        audioSrc.volume = Random.Range(volumeMin, volumeMax);
        audioSrc.Play();
        yield return new WaitForSeconds(0.4f);
        audioSrc.Stop();
        isPlaying = false;
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        animator.SetFloat("moveX", setVector.x);
        animator.SetFloat("moveY", setVector.y);
    }

    public void ChangeAnim(Vector2 direction)
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