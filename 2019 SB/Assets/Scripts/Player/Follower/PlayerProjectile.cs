using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Movement variables")]
    public float speed;

    [Header("Rigidbody")]
    public Rigidbody2D myRigidbody;

    [Header("Lifetime")]
    public float lifetime;
    private float timestamp = 0.0f;

    void Start()
    {
        timestamp = Time.time;
    }

    void Update()
    {
        if (Time.time > (timestamp + lifetime))
        {
            Destroy(this.gameObject);
        }
    }

    public void Setup(Vector2 velocity, Vector3 direction)
    {
        myRigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);   
    }

    //public void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("enemy"))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

}
