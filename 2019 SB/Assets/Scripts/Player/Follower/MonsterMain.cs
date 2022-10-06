using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMain : MonoBehaviour
{
    [Header("Monster Variables")]
    public CharacterState currentState;
    public float speed; //follow speed
    public float distance; //follows after x distance from player
    private Rigidbody2D myRigidbody;
    private Animator animator;
    private Vector3 change;
    public FloatValue currentHealth;

    //TODO ENERGY
    public FloatValue currentEnergy;
    public Message playerEnergyMessage;
    private bool isRegenEnergy = false;

    //TODO KNOCKBACK
    private bool isKnocked = false;
    //--
    private bool isAttacking = false;


    //TODO ABILITY
   //public GameObject playerProjectile;
    [SerializeField] private DashAbility dash = null;
    [SerializeField] private GenericTech projectile;

    private Vector2 facingDirection = Vector2.down;

    [Header("Game Variables")]
    private GameObject player;
    private GameObject follower;
    private GameObject gameManager;
    private float gameSpeed;
    private GameObject currentActiveChar;
    public Vector3 attackDirection;

    [Header("Particle Effect Variables")]
    public GameObject particleEffect;
    public float effectDelay;

    [Header("Camera Variables")]
    private CameraMovement cam;
    Camera pCamera;
    public Vector3 mousePosition;
    public Vector3 playerPosition;

    private float timestamp = 0.0f;
    private float magicTimestamp = 0.0f;

    void Start()
    {
        currentState = CharacterState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        follower = GameObject.FindWithTag("Follower");
        gameManager = GameObject.FindWithTag("GameManager");
        pCamera = Camera.main.GetComponent<Camera>();
        projectile = GetComponent<MonsterStats>().ownedTechs[1];

        if (Camera.main.GetComponent<CameraMovement>() != null)
        {
            cam = Camera.main.GetComponent<CameraMovement>();
        }
    }

    void Update()
    {
        currentActiveChar = gameManager.GetComponent<GameManager>().currentActiveChar;
        mousePosition = Input.mousePosition;
        playerPosition = pCamera.WorldToScreenPoint(follower.transform.position);
        attackDirection = mousePosition - playerPosition;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        gameSpeed = gameManager.GetComponent<GameManager>().GameSpeed.RuntimeValue;


        if (!isKnocked)
        {
            myRigidbody.velocity = Vector2.zero;
        }

        

        //TODO ENERGY
        if ((currentEnergy.RuntimeValue != currentEnergy.initialValue) && (Time.time > (magicTimestamp + 5.0f)) && gameSpeed != 0)
        {
            if (!isRegenEnergy)
            {
                StartCoroutine(RegainEnergyOverTime());
            }
        }

        if (gameSpeed != 0)
        {
            GetInput();
            animator.speed = 1f;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (currentActiveChar == follower && (currentState == CharacterState.walk || currentState == CharacterState.idle) && currentState != CharacterState.stagger)
            {
                UpdateAnimationAndMove();
            }

            if (currentActiveChar == player && (currentState == CharacterState.walk || currentState == CharacterState.idle) && player.GetComponent<PlayerMain>().currentState != CharacterState.interact)
            {
                FollowCharacter();
                //need to freeze power ups
                //animator.speed = 1f;
            }

            else if (player.GetComponent<PlayerMain>().currentState == CharacterState.interact)
            {
                animator.SetBool("moving", false);
                // animator.speed = 0f;
            }
        }else if (gameSpeed == 0)
        {
            animator.speed = 0f;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    //TOOD ENERGY
    private IEnumerator RegainEnergyOverTime()
    {
        isRegenEnergy = true;

        while (currentEnergy.RuntimeValue < currentEnergy.initialValue && (player.GetComponent<PlayerMain>().currentState != CharacterState.interact)
            && (Time.time > (magicTimestamp + 5.0f)))
        {
            currentEnergy.RuntimeValue += follower.GetComponent<MonsterStats>().WisdomStat;
            playerEnergyMessage.Raise();

            yield return new WaitForSeconds(1);
        }

        isRegenEnergy = false;
    }

    void GetInput()
    {
        if (gameManager.GetComponent<GameManager>().gameState == GameState.normal)
        {
            if (currentActiveChar == follower && Input.GetButtonDown("interact1") && (currentState == CharacterState.walk || currentState == CharacterState.idle)
                && currentState != CharacterState.attack && currentState != CharacterState.stagger)
            {
                ChangeAnim(attackDirection);
                if (!isAttacking)
                {
                    StartCoroutine(AttackCo());
                }
            }

            if (currentActiveChar == follower && Input.GetButtonDown("Dash") && (currentState == CharacterState.walk || currentState == CharacterState.idle)
                && currentState != CharacterState.attack)
            {
                if (dash)
                {
                    dash.dashForce = GetComponent<MonsterStats>().SpeedStat;
                    StartCoroutine(DashCo(dash.duration));
                }
            }

            if (currentActiveChar == follower && Input.GetButtonDown("Secondary Attack") && (currentState == CharacterState.walk || currentState == CharacterState.idle)
                && currentState != CharacterState.attack && currentState != CharacterState.stagger)
            {
                if (projectile)
                {
                    if (currentEnergy.RuntimeValue > follower.GetComponent<MonsterStats>().ownedTechs[1].energyCost)
                    {
                        ChangeAnim(attackDirection);
                        magicTimestamp = Time.time;
                        StartCoroutine(SecondAttackCo());

                        currentEnergy.RuntimeValue -= follower.GetComponent<MonsterStats>().ownedTechs[1].energyCost; //energy used and deducted by attack2
                        playerEnergyMessage.Raise();
                    }
                }

            }
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
            facingDirection = change;
        }
        else
        {
            currentState = CharacterState.idle;
            animator.SetBool("moving", false);
        }
    }

    void FollowCharacter()
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
        else if (change == Vector3.zero || (Vector3.Distance(transform.position, currentActiveChar.transform.position) < distance/1.1))
        {
            animator.SetBool("moving", false);
        }
    }

    public void SpawnParticleEffect(GameObject particleEffect, float EffectDelay)
    {
        if (particleEffect != null)
        {
            GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.identity);
            Destroy(effect, EffectDelay);
        }
    }

    public void Knock(float knockTime)
    {
        timestamp = Time.time;
        if (currentHealth.RuntimeValue > 0 && !isKnocked)
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else if (currentHealth.RuntimeValue <= 0)
        {
            //this.gameObject.SetActive(false); //game over precursor
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        isKnocked = true;

        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = CharacterState.idle;
            myRigidbody.velocity = Vector2.zero;
        }

        isKnocked = false;
    }

    private IEnumerator AttackCo()
    {
        isAttacking = true;

        animator.SetBool("1attacking", true);
        currentState = CharacterState.attack;
        yield return null;
        animator.SetBool("1attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != CharacterState.interact)
        {
            currentState = CharacterState.idle;
        }

        isAttacking = false;
    }

    private IEnumerator SecondAttackCo()
    {
        //TODO ABILITY change to 2attacking to avoid creating 1attack hitbox
        //animator.SetBool("1attacking", true);
        currentState = CharacterState.attack;
        yield return null;
        projectile.Tech(gameObject, "enemy", transform.position, facingDirection, mousePosition, attackDirection, animator, myRigidbody);
        //animator.SetBool("1attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != CharacterState.interact)
        {
            currentState = CharacterState.idle;
        }
    }

    public IEnumerator DashCo(float dashDuration)
    {
        currentState = CharacterState.dash;
        dash.Ability(transform.position, facingDirection, mousePosition, attackDirection, animator, myRigidbody);
        yield return new WaitForSeconds(dashDuration);
        currentState = CharacterState.idle;
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
