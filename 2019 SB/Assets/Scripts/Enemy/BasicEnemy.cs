using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class BasicEnemy : MonoBehaviour
{
    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Enemy Stats")]
    //public FloatValue maxHealth;
    //public FloatValue health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public Vector2 homePosition;
    public Rigidbody2D myRigidbody;

    [Header("Animator")]
    public Animator anim;

    [Header("Target Variables")]
    public GameObject target;
    public GameObject player;
    public float chaseRadius;
    public float attackRadius;

    
    void Start()
    {
        target = GameObject.FindWithTag("Follower");
        player = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        currentState = EnemyState.idle;
        anim = GetComponent<Animator>();
        anim.logWarnings = false;
        anim.SetBool("wakeUp", true);
        Invoke("UpdateHealth", 0.01f);
        transform.position = homePosition;
    }

    //private void UpdateHealth()
    //{
    //    health.RuntimeValue = gameObject.GetComponent<MonsterStats>().HPStat;
    //    Debug.Log(gameObject.name + " " + health.RuntimeValue);

    //}

    public void Knock(Rigidbody2D myRigidbody, float knockTime)
    {

        StartCoroutine(KnockCo(myRigidbody, knockTime));

    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            currentState = EnemyState.idle;
            //myRigidbody.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if ((currentState == EnemyState.idle || currentState == EnemyState.walk || currentState == EnemyState.attack) && currentState != EnemyState.stagger
            && player.GetComponent<PlayerMain>().currentState != CharacterState.interact)
        {
            CheckDistance();
            //anim.speed = 1f;
        }
        else if (player.GetComponent<PlayerMain>().currentState == CharacterState.interact)
        {
           //anim.speed = 0f;
        }

        if (anim.GetBool("moving") == false)
        {
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (anim.GetBool("moving") == true)
        {
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public virtual void CheckDistance()
    {
        if(Vector3.Distance(target.transform.position, transform.position) <= chaseRadius && Vector3.Distance(target.transform.position, transform.position) > attackRadius)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            ChangeAnim(temp - transform.position);
            //myRigidbody.MovePosition(temp);
            ChangeState(EnemyState.walk);
            anim.SetBool("wakeUp", true);
            anim.SetBool("moving", true);
        }
        else if (Vector3.Distance(target.transform.position, transform.position) > chaseRadius)
        {
            currentState = EnemyState.idle;
            anim.SetBool("wakeUp", false);
            anim.SetBool("moving", false);
        }
    }

    public virtual void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    public virtual void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);

            } if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }

        }

        if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
             if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
