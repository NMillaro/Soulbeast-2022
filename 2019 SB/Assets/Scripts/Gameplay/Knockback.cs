using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Knockback : MonoBehaviour
{
    [SerializeField] protected float thrust;
    [SerializeField] protected float knockTime;
    [SerializeField] protected string otherTag;

    public GameObject player;
    public GameObject follower;

    void FixedUpdate()
    {
        player = GameObject.FindWithTag("Player");
        follower = GameObject.FindWithTag("Follower");
    }


        private void OnTriggerEnter2D(Collider2D other)
        {
        /*
        if (other.gameObject.CompareTag("breakable") && gameObject.CompareTag("FollowerAttack"))
        {
            other.GetComponent<Pot>().Smash();
        }
        */

        if (other.gameObject.tag != "Untagged")
        {
            if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
                {
                    Rigidbody2D hit = other.GetComponentInParent<Rigidbody2D>();

                    if (hit != null)
                    {
                        Vector3 difference = hit.transform.position - transform.position;
                        difference = difference.normalized * thrust;
                        hit.DOMove(hit.transform.position + difference, knockTime);

                        if (other.gameObject.CompareTag("enemy") && other.isTrigger)
                        {
                            if (gameObject.CompareTag("FollowerAttack"))
                            {  
                                hit.GetComponent<BasicEnemyAI>().currentState = EnemyState.stagger;
                                other.GetComponentInParent<BasicEnemyAI>().Knock(hit, knockTime);
                                //Debug.Log("enemy hit");
                            }
                            else if (gameObject.CompareTag("enemy"))
                            {
                                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            }
                            //else
                            //    other.GetComponent<Enemy>().Knock(hit, 0, 0);

                        }
                        if (gameObject.CompareTag("enemyAttack"))
                        {
                            if (other.gameObject.CompareTag("Follower"))
                            {
                            if (other.enabled && other.isTrigger && (other.GetComponentInParent<MonsterMain>().currentState != CharacterState.stagger || other.GetComponent<MonsterMain>().currentState != CharacterState.interact))
                            {
                                hit.GetComponent<MonsterMain>().currentState = CharacterState.stagger;
                                other.GetComponentInParent<MonsterMain>().Knock(knockTime);
                                //Debug.Log("Monster hit");

                            }
                            else { }
                            }
                        }
                        if (other.gameObject.CompareTag("Player"))
                        {
                            if (other.isTrigger && (other.GetComponent<PlayerMain>().currentState != CharacterState.interact))
                            {
                                other.GetComponent<PlayerMain>().Knock(knockTime);
                                //Debug.Log("player hit");
                            }
                        }
                    }
                }
            }
        }
}


