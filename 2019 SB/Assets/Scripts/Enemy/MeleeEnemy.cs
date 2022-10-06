using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeEnemy : BasicEnemy
{
    public Collider2D boundary;

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= chaseRadius && Vector3.Distance(target.transform.position, transform.position) > attackRadius
            && boundary.bounds.Contains(target.transform.position))
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            ChangeAnim(temp - transform.position);
            //myRigidbody.MovePosition(temp);
            ChangeState(EnemyState.walk);
            anim.SetBool("moving", true);
        }
        else if (Vector3.Distance(target.transform.position, transform.position) <= chaseRadius && Vector3.Distance(target.transform.position, transform.position) <= attackRadius)
        {
            if ((currentState == EnemyState.idle || currentState == EnemyState.walk) && currentState != EnemyState.stagger && currentState != EnemyState.attack)
            {
                StartCoroutine(AttackCo());
            }
        }
        else if (Vector3.Distance(target.transform.position, transform.position) > chaseRadius*1.5 || !boundary.bounds.Contains(target.transform.position))
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, homePosition, moveSpeed * Time.deltaTime);

            if ((transform.position.x == homePosition.x) && (transform.position.y == homePosition.y))
            {
                currentState = EnemyState.idle;
                anim.SetBool("moving", false);
            }
            else
            {
                ChangeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                anim.SetBool("moving", true);
            }
        }
    }

    public IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(0.33f);
        anim.SetBool("1attacking", true);
        currentState = EnemyState.attack;
        yield return null;
        anim.SetBool("1attacking", false);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.idle;

    }

    public override void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    public override void ChangeAnim(Vector2 direction)
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

}
