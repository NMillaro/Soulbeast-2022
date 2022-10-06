using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    protected virtual void SpawnParticleEffect(GameObject particleEffect, float EffectDelay)
    {
        if (particleEffect != null)
        {
            GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.identity);
            Destroy(effect, EffectDelay);
        }
    }
}
