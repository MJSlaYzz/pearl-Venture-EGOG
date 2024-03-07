using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvent2 : MonoBehaviour
{
    #region Variables

    [Header("Event settings")]
    [SerializeField] private float progressValue = 1;
    private float startedValue;
    [SerializeField] private float decreaseAmount = 0.02f;
    [SerializeField] private float increaseAmount = 0.01f;
    private float timeThreshold = 0;
    [SerializeField] private Gradient imageColorGradient;
    [SerializeField] private Image ringImage;
    [SerializeField] private Image letterImage;
    [SerializeField] private Sprite qSprite;
    [SerializeField] private Sprite eSprite;
    [SerializeField] private GameObject successScreen;
    [SerializeField] private GameObject failScreen;
    public bool eventStarted = false;
    private PlayerMovementFour playerMovement;
    private bool isFail = false;
    public bool playerGotStuck = false;
    [HideInInspector] PlayerHealth playerHealth;
    private bool canRepeatEvent = true;
    [HideInInspector] public bool eventSucceeded = false; //for ghost nets
    [HideInInspector] public bool onCooldown = false;
    [HideInInspector] public bool readyToCut = false;
    [SerializeField] float netCooldown = 2f;

    //[Header("UnLock")]
    //public Image lockImage;
    //public Sprite openedLockImage;
    //public Sprite closedLockImage;
    //public Animation lockAnimation;
    //public AudioSource audioSource;
    //public AudioClip unlockAudioClip;
    //public AudioClip lockStuckAudioClip;

    #endregion

    private void Start()
    {
        letterImage.enabled = false;
        ringImage.enabled = false;
        startedValue = progressValue;
        playerMovement = FindObjectOfType<PlayerMovementFour>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerGotStuck)
        {
            StartEvent();
            playerGotStuck = false;
        }
        if (eventStarted)
        {
            CheckInput();
            TimeThresholdUpdate();
            UpdateImage();

            if (progressValue <= 0)
            {
                EventFailed();
            }
            else if (progressValue >= startedValue)
            {
                EventSucceeded();
            }
        }
    }
    public void StartEvent() 
    {
        StopAllCoroutines();
        ChooseImage();
        letterImage.enabled = true;
        ringImage.enabled = true;
        eventStarted = true;
        //playerMovement.enabled = false;
        playerMovement.playerCanMove = false;
        progressValue = startedValue / 2;
        successScreen.SetActive(false);
        failScreen.SetActive(false);
        isFail = false;
        canRepeatEvent = true;
        //Nets
        onCooldown = false;
        readyToCut = false;
        eventSucceeded = false;

    }
    private void RepeatEvent()
    {
        StopAllCoroutines();
        ChooseImage();
        letterImage.enabled = true;
        ringImage.enabled = true;
        eventStarted = true;
        playerMovement.playerCanMove = false;
        progressValue = startedValue / 2;
        successScreen.SetActive(false);
        failScreen.SetActive(false);
        isFail = false;
        //eventSucceeded = false;
    }
    private void CheckInput()
    {
        if(letterImage.sprite == qSprite)
        {
            if (Input.GetKeyDown("q"))
            {
                //Debug.Log("Q was pressed!");
                progressValue += increaseAmount;
            }
        }
        else if(letterImage.sprite = eSprite)
        {
            if (Input.GetKeyDown("e"))
            {
                //Debug.Log("E was pressed!");
                progressValue += increaseAmount;
            }
        }

    }
    private void TimeThresholdUpdate()
    {
        timeThreshold += Time.deltaTime;
        if (timeThreshold > 0.1f)
        {
            timeThreshold = 0;
            progressValue -= decreaseAmount;
        }
    }
    private void ChooseImage()
    {
        int index = Random.Range(0, 9);

        //print("Index = " + index);
        if(index <= 4)
        {
            letterImage.sprite = qSprite;
        }
        else
        {
            letterImage.sprite = eSprite;
        }
    }
    private void UpdateImage()
    {
        ringImage.fillAmount = progressValue;
        ringImage.color = imageColorGradient.Evaluate(progressValue);
    }
    private void EventFailed()
    {
        letterImage.enabled = false;
        ringImage.enabled = false;
        eventStarted = false;
        //Debug.Log("Event Failed!");
        //failScreen.SetActive(true);
        //successScreen.SetActive(false);
        isFail = true;
        //StartCoroutine(ReleasePlayer());
        eventSucceeded = false;
        ReleasePlayer2();

    }
    private void EventSucceeded()
    {
        // NETs
        onCooldown = true;
        readyToCut = true;
        eventSucceeded = true;
        StartCoroutine(StartCooldown());
        letterImage.enabled = false;
        ringImage.enabled = false;
        eventStarted = false;
        //Debug.Log("Event Succeeded!");
        //successScreen.SetActive(true);
        //failScreen.SetActive(false);
        isFail = false;
        //StartCoroutine(ReleasePlayer());
        ReleasePlayer2();
    }
    void ReleasePlayer2()
    {
        if (!isFail)
        {
            if (playerMovement != null)
            {
                playerHealth.TurnOffPlayerGotStuck();
                playerMovement.playerCanMove = true;
            }
            else
            {
                print("Player movement script can't be found!");
            }
        }
        else
        {
            if (canRepeatEvent)
            {
                RepeatEvent();
            }

        }
    }
    public void ResetEvent()
    {
        canRepeatEvent = false;
        letterImage.enabled = false;
        ringImage.enabled = false;
        eventStarted = false;
        //Debug.Log("Event RESET!");
        isFail = false;
        playerMovement.playerCanMove = true;
    }
    IEnumerator StartCooldown() // temCooldown should = cooldown && startCooldownStart should = true;
    {
        //print("cooldown started");
        yield return new WaitForSeconds(netCooldown);
        onCooldown = false;
    }
    //private IEnumerator ReleasePlayer()
    //{
    //    if (!isFail)
    //    {
    //        lockImage.sprite = closedLockImage;
    //        lockImage.enabled = true;
    //        yield return new WaitForSecondsRealtime(1f);
    //        lockImage.sprite = openedLockImage;
    //        audioSource.clip = unlockAudioClip;
    //        audioSource.Play();
    //        yield return new WaitForSecondsRealtime(0.5f);
    //        lockImage.enabled = false;
    //        if (playerMovement != null)
    //        {
    //            //playerMovement.enabled = true;
    //            playerMovement.playerCanMove = true;
    //        }
    //        else
    //        {
    //            print("Player movement script can't be found!");
    //        }
    //    }
    //    else
    //    {
    //        lockImage.sprite = closedLockImage;
    //        lockImage.enabled = true;
    //        lockAnimation.Play();
    //        audioSource.clip = lockStuckAudioClip;
    //        audioSource.Play();
    //        yield return new WaitForSecondsRealtime(1.5f);
    //        lockImage.enabled = false;
    //        StartEvent();
    //        //if (playerMovement != null)
    //        //{
    //        //    //playerMovement.enabled = true;
    //        //    playerMovement.playerCanMove = true;
    //        //}
    //        //else
    //        //{
    //        //    print("Player movement script can't be found!");
    //        //}
    //    }

    //}
}
