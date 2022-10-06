using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFrame : MonoBehaviour
{
    [Header("IFrames")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    private void OnEnable()
    {
        //mySprite = GetComponentInParent<SpriteRenderer>();
        //triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider != null)
        {
            triggerCollider.enabled = true;
            mySprite.color = regularColor;
        }
    }

    public void CallFlashCo()
    {
        StartCoroutine(FlashCo());
    }


    private IEnumerator FlashCo()
    {
        int temp = 0;
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }
        while (temp < numOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        if (triggerCollider != null)
        {
            triggerCollider.enabled = true;
        }
    }
}
