using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnemy : BasicEnemy
{
    public Collider2D boundary;

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= chaseRadius && Vector3.Distance(target.transform.position, transform.position) > attackRadius
            && boundary.bounds.Contains(target.transform.position))
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            ChangeAnim(temp - transform.position);
            myRigidbody.MovePosition(temp);
            ChangeState(EnemyState.walk);
            anim.SetBool("wakeUp", true);
        }
        else if (Vector3.Distance(target.transform.position, transform.position) > chaseRadius || !boundary.bounds.Contains(target.transform.position))
        {
            anim.SetBool("wakeUp", false);
        }
    }


}
