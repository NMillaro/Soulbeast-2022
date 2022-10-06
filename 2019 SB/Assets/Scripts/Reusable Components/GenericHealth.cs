using UnityEngine;

public class GenericHealth : MonoBehaviour
{
    public float maxHealth;
    [SerializeField] public float currentHealth;


    void Start()
    {
        currentHealth = maxHealth;       
    }

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
    }


    public virtual void Heal(float amountToHeal)
    {
        currentHealth += amountToHeal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public virtual void FullHeal()
    {
        currentHealth = maxHealth;
    }

    public virtual void Damage(float amountToDamage)
    {
        currentHealth -= amountToDamage;
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public virtual void InstantDeath()
    {
        currentHealth = 0;
    }



}
