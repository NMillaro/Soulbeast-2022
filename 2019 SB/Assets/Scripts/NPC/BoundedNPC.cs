using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedNPC : Sign
{
    [Header("Game Speed")]
    public float gameSpeed;

    private Vector3 directionVector;
    private Transform myTransform;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    public Collider2D boundary;
    private bool isMoving;
    public float minMoveTime;
    public float maxMoveTime;
    private float moveTimeSeconds;
    public float minWaitTime;
    public float maxWaitTime;
    public Vector3 facePlayerDirection;


    private float waitTimeSeconds;

    protected override void Start()
    {
        base.Start();
        moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
        waitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        ChangeDirection();
        
    }


    protected override void Update()
    {
        base.Update();
        
        facePlayerDirection = playerPosition - objectPosition;

        gameSpeed = gameManager.GetComponent<GameManager>().GameSpeed.RuntimeValue;

        if (gameSpeed != 0)
        {
            anim.speed = 1f;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (isMoving)
            {
                moveTimeSeconds -= Time.deltaTime;

                if (moveTimeSeconds <= 0)
                {
                    moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
                    isMoving = false;
                }
                if (!playerInRange)
                {
                    Move();
                    anim.SetBool("moving", true);
                    ChangeAnim(directionVector);
                }
                else
                {
                    anim.SetBool("moving", false);
                    ChangeAnim(facePlayerDirection);
                }

            }

            else
            {
                waitTimeSeconds -= Time.deltaTime;
                if (waitTimeSeconds <= 0)
                {
                    if (!playerInRange)
                    {
                        ChooseDifferentDirection();
                    }
                    isMoving = true;
                    waitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
                }

                anim.SetBool("moving", false);
                myRigidbody.velocity = Vector2.zero;
            }
        }
        else if (gameSpeed == 0)
        {
            anim.speed = 0f;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void ChooseDifferentDirection()
    {
        Vector3 temp = directionVector;
        ChangeDirection();

        int loops = 0;
        while (temp == directionVector && loops < 100)
        {
            loops++;
            ChangeDirection();
        }
    }

    private void Move()
    {
        Vector3 temp = myTransform.position + directionVector * speed * Time.deltaTime;
        if (boundary.bounds.Contains(temp))
        {
            myRigidbody.MovePosition(temp);
        }
        else
        {
            ChangeDirection();
        }

    }

    void ChangeDirection()
    {
        int direction = Random.Range(0, 4);

        switch (direction)
        {
            case 0:
                directionVector = Vector3.right;
                break;
            case 1:
                directionVector = Vector3.up;
                break;
            case 2:
                directionVector = Vector3.left;
                break;
            case 3:
                directionVector = Vector3.down;
                break;
            default:
                break;
        }
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        anim.SetFloat("moveX", directionVector.x);
        anim.SetFloat("moveY", directionVector.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ChooseDifferentDirection();

    }

    private void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
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
