using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);

        return damagePopup;
    }

    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private float fadeTimer = 0;
    [SerializeField] private Color textColor;
    [SerializeField] private float normalFontSize = 0;
    [SerializeField] private float criticalFontSize = 0;
    [SerializeField] private float moveYSpeed = 0;
    [SerializeField] private float fadeSpeed = 0;
    [SerializeField] private float scaleAmount = 0;
    private static int sortingOrder;

private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit)
        {
            textMesh.fontSize = normalFontSize;
        }
        else
        {
            textMesh.fontSize = criticalFontSize;
        }
        textMesh.color = textColor;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        if (fadeTimer > fadeTimer * .5f)
        {
            transform.localScale += Vector3.one * scaleAmount * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * scaleAmount * Time.deltaTime;
        }

        if (sortingOrder >= 100)
        {
            sortingOrder = 0;
        }

        fadeTimer -= Time.deltaTime;
        if (fadeTimer < 0)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
