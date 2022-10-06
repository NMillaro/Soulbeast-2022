using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Projectile Ability", fileName = "New Projectile Ability")]


public class ProjectileAbility : GenericAbility
{
    [SerializeField] private GameObject thisProjectile = null;

    public override void Ability(Vector2 monsterPosition, Vector2 monsterFacingDirection, 
        Vector3 mousePosition, Vector3 attackDirection, 
        Animator monsterAnimator = null, Rigidbody2D monsterRigidBody = null)
    {
        float projectileRotation = Mathf.Atan2(attackDirection.y, attackDirection.x)*Mathf.Rad2Deg;
        GameObject newProjectile = Instantiate(thisProjectile, monsterPosition, 
            Quaternion.Euler(0f, 0f, projectileRotation));

        GenericProjectile temp = newProjectile.GetComponent<GenericProjectile>();

        if (temp)
        {
            temp.Setup(attackDirection);
        }

    }


}
