using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class TechDamage : GenericDamage
{
    public GenericTech tech;
    

    protected override void Start()
    {
        base.Start();
        if (transform.parent)
        {
            thisMonster = transform.parent.gameObject;
            tech = GetComponentInParent<MonsterStats>().ownedTechs[0];
        }
    }

    private void DamageCalc(int level, MonsterType techType, float power, int attack, int def)
    {
        damage = (int)((2 * level / 5)+2 * (power * (attack / def))/ 50)+2;
    }


    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Untagged")
        {
            if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
            {
                GenericHealth temp = other.GetComponent<GenericHealth>();

                if (temp)
                {
                    bool isCritical = false;

                    if (tech.category == TechCategory.Physical)
                    {
                        float powerTemp = powerValue(tech, tech.power);
                       // Debug.Log(tech.power + " : " + powerTemp);
                        DamageCalc(thisMonster.GetComponent<MonsterStats>().currentLevel,
                            tech.techType, powerTemp, thisMonster.GetComponentInParent<MonsterStats>().AttackStat, 
                            other.GetComponentInParent<MonsterStats>().DefenceStat);

                        float multiplier = moveEffectiveness(tech, other);
                        float final = multiplier * damage;
                        final = Random.Range(final * 0.9f, final * 1.1f);
                        temp.Damage((int)final);
                        //Debug.Log("Damage: " + (int)final);
                        DamagePopup.Create(other.transform.position, (int)final, isCritical);
                    }
                    else if (tech.category == TechCategory.Magical)
                    {
                        float powerTemp = powerValue(tech, tech.power);
                       // Debug.Log(tech.power + " : " + powerTemp);
                        DamageCalc(thisMonster.GetComponent<MonsterStats>().currentLevel,
                           tech.techType,
                           powerTemp, thisMonster.GetComponentInParent<MonsterStats>().MagicStat, 
                           other.GetComponentInParent<MonsterStats>().WillpowerStat);

                        float multiplier = moveEffectiveness(tech, other);
                        float final = multiplier * damage;
                        final = Random.Range(final * 0.9f, final * 1.1f);
                        temp.Damage((int)final);
                      //  Debug.Log("Damage: " + (int)final);
                        DamagePopup.Create(other.transform.position, (int)final, isCritical);
                    }

                    float powerValue(GenericTech tech, float power)
                    {
                        if (tech.techType != MonsterType.Neutral)
                        {
                            if (tech.techType == thisMonster.GetComponent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonster.type1 ||
                                tech.techType == thisMonster.GetComponent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonster.type2)
                            {
                                power = power * 1.5f;
                            }
                        }
                        return power;
                    }

                    float moveEffectiveness(GenericTech tech, Collider2D otherMon)
                    {
                        float result = MonsterData.TypeEffectiveness[tech.techType.ToString()]
                            [other.GetComponentInParent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonster.type1.ToString()];
                        if (other.GetComponentInParent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonster.type2.ToString() != "Neutral")
                            result *= MonsterData.TypeEffectiveness[tech.techType.ToString()]
                                [other.GetComponentInParent<MonsterStats>().thisMonster.GetComponent<ThisMonster>().thisMonster.type2.ToString()];
                        return result;
                    }

                }
            }
        }
    }
}
