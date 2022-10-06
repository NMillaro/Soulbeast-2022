using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
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
}
