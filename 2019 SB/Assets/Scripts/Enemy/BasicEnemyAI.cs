using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BasicEnemyAI : MonoBehaviour
{
    [Header("Game Manager")]
    public GameObject gm;
    public float gameSpeed;

    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Target Variables")]
    public GameObject player;
    public GameObject target;
    public Vector2 tempTarget;
    public float chaseRadius;
    public float attackRadius;
    private bool isAttacking = false;

    public Vector3 homePosition;
    public Collider2D boundary;

    [Header("Animator")]
    public Animator anim;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;
    
    void OnEnable()
    {
        gm = GameObject.FindWithTag("GameManager");
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Follower");
        player = GameObject.FindWithTag("Player");
        currentState = EnemyState.idle;
        anim = GetComponent<Animator>();
        //anim.logWarnings = false;
        //anim.SetBool("wakeUp", true);
        //Invoke("UpdateHealth", 0.01f);
        homePosition = transform.position;

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && gameSpeed != 0)
        {
            seeker.StartPath(rb.position, tempTarget, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        gameSpeed = gm.GetComponent<GameManager>().GameSpeed.RuntimeValue;

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }


        if ((currentState == EnemyState.idle || currentState == EnemyState.walk || currentState == EnemyState.attack) && currentState != EnemyState.stagger
            && gameSpeed != 0)
        {
            anim.speed = 1f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            CheckDistance();
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
            
        }
        else if(gameSpeed == 0)
        {
            anim.speed = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (anim.GetBool("moving") == false)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (anim.GetBool("moving") == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }

    public void CheckDistance()
    {
        Vector3 temp = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        if (Vector3.Distance(target.transform.position, transform.position) <= chaseRadius && Vector3.Distance(target.transform.position, transform.position) > attackRadius
            && boundary.bounds.Contains(target.transform.position))
        {
            tempTarget = target.transform.position;
            ChangeAnim(temp - transform.position);
            ChangeState(EnemyState.walk);
            anim.SetBool("moving", true);
        }
        else if (Vector3.Distance(target.transform.position, transform.position) <= chaseRadius && Vector3.Distance(target.transform.position, transform.position) <= attackRadius)
        {
            if ((currentState == EnemyState.idle || currentState == EnemyState.walk) && currentState != EnemyState.stagger && currentState != EnemyState.attack)
            {
                ChangeAnim(temp - transform.position);
                StartCoroutine(AttackCo());
            }
        }
        else if (Vector3.Distance(target.transform.position, transform.position) > chaseRadius * 1.5 || !boundary.bounds.Contains(target.transform.position))
        {
            if ((transform.position.x == homePosition.x) && (transform.position.y == homePosition.y))
            {
                currentState = EnemyState.idle;
                anim.SetBool("moving", false);
            }
            else
            {
                ChangeAnim(homePosition - transform.position);
                tempTarget = homePosition;
                ChangeState(EnemyState.walk);
                anim.SetBool("moving", true);
            }
        }
    }

    public IEnumerator AttackCo()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            //TODO change wait to separate attack speed variable
            yield return new WaitForSeconds(0.1f);
            anim.SetBool("1attacking", true);
            currentState = EnemyState.attack;
            yield return null;
            anim.SetBool("1attacking", false);
            yield return new WaitForSeconds(0.5f);
            currentState = EnemyState.idle;
            isAttacking = false;
        }

    }

    public void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    public void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);

            }
            if (direction.x < 0)
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
            myRigidbody.velocity = Vector2.zero;
        }
    }
}
