using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : BasicEnemy
{
    public GameObject enemyProjectile;
    public float fireDelay;
    private float fireDelaySeconds;
    public Vector3 attackDirection;

    public bool canFire = true;

    private void Update()
    {
        attackDirection = target.transform.position - transform.position;
        fireDelaySeconds -= Time.deltaTime;
        if (fireDelaySeconds <= 0)
        {
            canFire = true;
            fireDelaySeconds = fireDelay;
        }
    }

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= chaseRadius && Vector3.Distance(target.transform.position, transform.position) > attackRadius)
        {
            if (canFire)
            {
                Vector3 temp = attackDirection;
                Projectile projectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
                projectile.Setup(temp, ProjectileDirection());
                canFire = false;
                ChangeState(EnemyState.attack);
                anim.SetBool("wakeUp", true);
            }
        }
        else if (Vector3.Distance(target.transform.position, transform.position) > chaseRadius)
        {
            anim.SetBool("wakeUp", false);
        }
    }

    Vector3 ProjectileDirection()
    {
        float temp = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

}
