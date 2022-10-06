using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Techs/Generic Tech", fileName = "New Generic Tech")]
[System.Serializable]

public class GenericTech : ScriptableObject
{
    public string techName;
    public TechCategory category;
    public MonsterType techType;
    public float energyCost;
    public float power;
    public string otherTag = "";
    public GameObject thisMonster;

    public virtual void Tech(GameObject thisMonster, string otherTag, Vector2 monsterPosition, Vector2 monsterFacingDirection,
        Vector3 mousePosition, Vector3 attackDirection, Animator monsterAnimator = null, Rigidbody2D monsterRigidBody = null)
    {

    }
}

public enum TechCategory
{
    Physical,
    Magical,
    Condition
}
