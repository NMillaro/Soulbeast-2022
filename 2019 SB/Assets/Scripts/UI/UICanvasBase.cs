using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasBase : MonoBehaviour
{
    protected bool textTyping = false;
    [SerializeField] protected FloatValue typingDelay;
    public GameObject dialogueBox;
    protected string currentText;
    public Text dialogueText;
    public string dialogue;
    private AudioSource audioSrc;
    public AudioClip typeClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GameObject.Find("SpeechSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IEnumerator ShowText()
    {
        textTyping = true;

        for (int i = 0; i <= dialogue.Length; i++)
        {
            currentText = dialogue.Substring(0, i);
            dialogueText.text = currentText;
            audioSrc.PlayOneShot(typeClip);
            yield return new WaitForSeconds(typingDelay.initialValue);
        }

        yield return new WaitForSeconds(0.25f); //delay after text finished
        textTyping = false;
    }
}

