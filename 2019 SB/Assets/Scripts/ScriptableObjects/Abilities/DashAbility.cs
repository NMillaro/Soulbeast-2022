using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Dash Ability", fileName = "Dash Ability")]
public class DashAbility : GenericAbility
{
    public float dashForce;

    public override void Ability(Vector2 monsterPosition, Vector2 monsterFacingDirection, Vector3 mousePosition,
        Vector3 attackDirection, Animator monsterAnimator = null, Rigidbody2D monsterRigidBody = null)
    {
        if (monsterRigidBody)
        {
            Vector3 dashVector = monsterRigidBody.transform.position + 
                (Vector3)monsterFacingDirection.normalized * dashForce;
            monsterRigidBody.DOMove(dashVector, duration);
        }

    }


}
