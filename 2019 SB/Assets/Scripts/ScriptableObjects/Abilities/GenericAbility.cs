using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Generic Ability", fileName = "New Generic Ability")]

public class GenericAbility : ScriptableObject
{
    public float energyCost;
    public float power;
    public float duration;

    public FloatValue followerEnergy;
    public Message useFollowerEnergy;

    public virtual void Ability(Vector2 monsterPosition, Vector2 monsterFacingDirection, 
        Vector3 mousePosition, Vector3 attackDirection, Animator monsterAnimator = null, Rigidbody2D monsterRigidBody = null)
    {

    }


}
