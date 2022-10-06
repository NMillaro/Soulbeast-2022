using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : MonoBehaviour
{
    [Header("Target Flash info")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;
    public bool isFlashing = false;

    private void OnEnable()
    {
          mySprite.color = regularColor;       
    }

    public void CallFlashCo()
    {
        StartCoroutine(FlashCo());
    }


    private IEnumerator FlashCo()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            int temp = 0;
            while (temp < numOfFlashes)
            {
                mySprite.color = flashColor;
                yield return new WaitForSeconds(flashDuration);
                mySprite.color = regularColor;
                yield return new WaitForSeconds(flashDuration);
                temp++;
            }
            isFlashing = false;
        }
    }
}
