using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("New Scene Variables")]
    public string sceneToLoad;
    public Vector2 newPosition;
    public VectorValue playerPosition;

    [Header("Transition Variables")]
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;
    private GameObject player;
    private GameObject follower;
    private GameObject gm;
    private GameObject currentActiveChar;
    public Name lastActiveChar;

    [Header("Sound Effects")]
    public AudioClip transitionClip;
    private AudioSource audioSrc;

    private void Awake()
    {
        if(fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1); 
        }
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        follower = GameObject.FindWithTag("Follower");
        gm = GameObject.FindWithTag("GameManager");
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        currentActiveChar = GameObject.FindWithTag(lastActiveChar.initialName);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject == player || other.gameObject == follower) && !other.isTrigger && (other.gameObject == currentActiveChar))
        {
            playerPosition.initialValue = newPosition;
            if (other.gameObject == player)
            {
                other.GetComponent<PlayerMain>().currentState = CharacterState.interact;
            }else if (other.gameObject == follower)
            {
                other.GetComponent<MonsterMain>().currentState = CharacterState.interact;
            }
            other.GetComponent<Animator>().SetBool("moving", false);
            StartCoroutine(FadeCo());
            audioSrc.PlayOneShot(transitionClip);
        }
    }

    public IEnumerator FadeCo()
    {
        if (fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(fadeWait);
       
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

}
