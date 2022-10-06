using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Techs/Projectile Tech", fileName = "New Projectile Tech")]

public class ProjectileTech : GenericTech
{
    [SerializeField] private GameObject thisProjectile = null;

    public override void Tech(GameObject thisMonster, string otherTag, Vector2 monsterPosition, Vector2 monsterFacingDirection,
        Vector3 mousePosition, Vector3 attackDirection,
        Animator monsterAnimator = null, Rigidbody2D monsterRigidBody = null)
    {
        float projectileRotation = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        GameObject newProjectile = Instantiate(thisProjectile, monsterPosition,
            Quaternion.Euler(0f, 0f, projectileRotation));

        GenericProjectile temp = newProjectile.GetComponent<GenericProjectile>();
        newProjectile.GetComponent<TechDamage>().tech = this;
        newProjectile.GetComponent<TechDamage>().otherTag = otherTag;
        newProjectile.GetComponent<TechDamage>().thisMonster = thisMonster;

        if (temp)
        {
            temp.Setup(attackDirection);
        }

    }
}
