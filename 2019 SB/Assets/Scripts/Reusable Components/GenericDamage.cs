using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GenericDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] public string otherTag;
    [SerializeField] public GameObject thisMonster;

    protected virtual void Start()
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Untagged") {
            if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
            {
                GenericHealth temp = other.GetComponent<GenericHealth>();

                if (temp)
                {
                    temp.Damage(damage);
                }
            }
        }
    }

}
