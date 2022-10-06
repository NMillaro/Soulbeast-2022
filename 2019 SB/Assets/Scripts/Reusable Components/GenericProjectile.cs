using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GenericProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private float speed = 0;

    [Header("Sound Effects")]
    public AudioClip shootClip;
    private AudioSource audioSrc;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        audioSrc.PlayOneShot(shootClip);
    }

     void Update()
    {
        //gamespeed
    }

    public void Setup(Vector2 velocity)
    {
        myRigidbody.velocity = velocity.normalized * speed;
        // transform.rotation = Quaternion.Euler(direction);
    }

}
