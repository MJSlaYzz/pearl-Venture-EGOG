using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesControllerNew : MonoBehaviour
{
    [HideInInspector] private bool shouldLerp = true;
    [HideInInspector] private float timer;
    [HideInInspector] private float smoothTime; // the amount of time it takes for the oxygen to finish
    [SerializeField] private float lowSmoothTime = 10f;
    [SerializeField] private float highSmoothTime = 15f;

    [HideInInspector] private float lerpValue;
    [SerializeField] private float numToLerpFrom = 50;
    [SerializeField] private float openSpeed = 5f;
    [SerializeField] private float closeSpeed = 5f;


    [SerializeField] private CollectablesCounter collectablesCounter;
    [SerializeField] private ParticleSystem plusOneParticle;

    [HideInInspector] private SpriteRenderer currenSprite;
    [SerializeField] private SpriteRenderer minimapIconcurrenSprite;
    [SerializeField] private Sprite closedOyster;
    [SerializeField] private Sprite emptyOyster;
    [SerializeField] private Sprite pearlOyster;

    [SerializeField] private AudioSource collectSFX;


    private bool canBeRepeated = true;
    private bool isCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        smoothTime = Random.Range(lowSmoothTime, highSmoothTime);
        currenSprite = this.GetComponent<SpriteRenderer>(); //getting it from the script insted of the inspector
        currenSprite.sprite = closedOyster;
        collectSFX = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(minimapIconcurrenSprite != null)
        {
            minimapIconcurrenSprite.sprite = currenSprite.sprite;
        }

        if (canBeRepeated)
        {
            LerpDown();
            LerpUp();
        }
        if (isCollected == true)
        {
            canBeRepeated = false;
            if(currenSprite.sprite != emptyOyster)
            {
                currenSprite.sprite = emptyOyster;
                minimapIconcurrenSprite.sprite = emptyOyster;
            }
        }
    }
    void LerpDown()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (shouldLerp && timer >= 0)
        {
            timer += 1 * Time.deltaTime * openSpeed;

            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            lerpValue = Mathf.Lerp(numToLerpFrom, 0, inverseT);
        }
        if (lerpValue <= 0)
        {
            shouldLerp = false;
            currenSprite.sprite = pearlOyster;
        }
    }
    void LerpUp()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (!shouldLerp && timer >= 0)
        {
            timer -= 1 * Time.deltaTime * closeSpeed;

            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            lerpValue = Mathf.Lerp(numToLerpFrom, 0, inverseT);
        }
        if (lerpValue >= numToLerpFrom)
        {
            shouldLerp = true;
            currenSprite.sprite = closedOyster;
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected == false && currenSprite.sprite == pearlOyster) //makes sure it wasn't already collected
        {
            if (collision.gameObject.tag == "Player")
            {
                plusOneParticle.Play();
                currenSprite.sprite = emptyOyster;
                isCollected = true; //can't be collected anymore
                collectablesCounter.AddAPearl();
                Debug.Log("Add one more pearl");        
            }
        }
    }
    */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isCollected == false && currenSprite.sprite == pearlOyster) //makes sure it wasn't already collected
        {
            if (collision.gameObject.tag == "Player")
            {
                plusOneParticle.Play();
                collectSFX.Play();
                currenSprite.sprite = emptyOyster;
                isCollected = true; //can't be collected anymore
                collectablesCounter.AddAPearl();
                //Debug.Log("Add one more pearl");
            }
        }
    }
}
