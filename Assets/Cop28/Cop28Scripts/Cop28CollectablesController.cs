using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop28CollectablesController : MonoBehaviour
{

    [HideInInspector] QuickTimeEvent3 qTE3;
    [SerializeField] private Cop28CollectablesCounter collectablesCounter;
    [SerializeField] private ParticleSystem plusOneParticle;
    [SerializeField] private GameObject eIcon;
    [HideInInspector] private SpriteRenderer currenSprite;
    [SerializeField] private SpriteRenderer minimapIconcurrenSprite;
    [SerializeField] private Sprite closedOyster;
    [SerializeField] private Sprite emptyOyster;
    [SerializeField] private Sprite pearlOyster;

    [HideInInspector] private PlayerMovementFour playerMovement;

    [SerializeField] private AudioSource collectSFX;


    private bool isCollected = false;
    private bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementFour>();
        eIcon.SetActive(false);
        qTE3 = FindObjectOfType<QuickTimeEvent3>();
        currenSprite = this.GetComponent<SpriteRenderer>(); //getting it from the script insted of the inspector
        currenSprite.sprite = closedOyster;
        collectSFX = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collided && !isCollected)
        {
            eIcon.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                eIcon.SetActive(false);
                if (!qTE3.eventStarted)
                {
                    qTE3.StartEvent();
                }
            }
        }
        else
        {
            eIcon.SetActive(false);
        }
        if (minimapIconcurrenSprite != null)
        {
            minimapIconcurrenSprite.sprite = currenSprite.sprite;
        }
        PearlEventUpdate();
    }
    void PearlEventUpdate()
    {
        if (collided && !isCollected)
        {
            CheckIfEventWasSuccessful();
            if (qTE3.canBePressed && !isCollected)
            {
                currenSprite.sprite = pearlOyster;
            }
            else if(!qTE3.canBePressed && !isCollected)
            {
                currenSprite.sprite = closedOyster;
            }
        }
    }
    void CheckIfEventWasSuccessful()
    {
        if (qTE3.hitTheTarget && !isCollected)
        {
            if (playerMovement != null)
            {
                playerMovement.isCutting = true;
            }
            isCollected = true; //can't be collected anymore
            plusOneParticle.Play();
            collectSFX.Play();
            currenSprite.sprite = emptyOyster;
            minimapIconcurrenSprite.sprite = emptyOyster;
            collectablesCounter.AddAPearl();
            //StartCoroutine(ResetAnimation());
            StartCoroutine(ResetBool());
            Debug.Log("Add one more pearl");
        }
    }
    private IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(0.2f);
        qTE3.ResetBoolens();
    }
    //private IEnumerator ResetAnimation()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    if (playerMovement != null)
    //    {
    //        playerMovement.isCutting = false;
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collided = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collided = false;
        }
    }

    
}
