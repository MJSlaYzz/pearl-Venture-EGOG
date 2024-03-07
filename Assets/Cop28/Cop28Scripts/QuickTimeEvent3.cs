using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuickTimeEvent3 : MonoBehaviour
{
    #region Variables

    [Header("Event settings")]
    [SerializeField] private Image mainCircle;
    [SerializeField] private Image target;
    [SerializeField] private Image movingImage;
    [SerializeField] private TextMeshProUGUI pressButtonText;
    [SerializeField] private float minRotaionSpeed = 10f;
    [SerializeField] private float maxRotaionSpeed = 20f;
    [HideInInspector] private float rotaionSpeed;
    [SerializeField] private float differenceBetweenTargetAndMovingImage = 7f;
    [SerializeField] private int tries = 2;
    [HideInInspector] private int triesIndex;
    [HideInInspector] public bool hitTheTarget = false;
    [HideInInspector] public bool eventFailed = false;
    [HideInInspector] public bool eventStarted = false;
    [HideInInspector] public bool canBePressed = false;
    [HideInInspector] private bool choseRandomSpeed = false;
    [HideInInspector] private PlayerMovementFour playerMovement;

    [Header("UnLock")]
    public Image lockImage;
    public Sprite openedLockImage;
    public Sprite closedLockImage;
    public Animation lockAnimation;
    public AudioSource audioSource;
    public AudioClip unlockAudioClip;
    public AudioClip lockStuckAudioClip;
    public AudioClip wrongAudioClip;
    public AudioClip correctAudioClip;

    #endregion

    private void Start()
    {
        SetTargetPostion();
        triesIndex = tries;
        playerMovement = FindObjectOfType<PlayerMovementFour>();
    }
    // Update is called once per frame
    void Update()
    {
        if (eventStarted)
        {
            if (!hitTheTarget && !eventFailed)
            {
                ChooseRandomSpeed();
                RotateMovingImage();
                CheckIfTargetIsTouchingMovingImage();
                //playerMovement.enabled = false;
                playerMovement.playerCanMove = false;
            }
            else
            {
                eventStarted = false;
                //playerMovement.enabled = true;
                playerMovement.playerCanMove = true;
            }
        }
        else
        {
            eventStarted = false;
            mainCircle.enabled = false;
            target.gameObject.SetActive(false);
            movingImage.gameObject.SetActive(false);
            pressButtonText.enabled = false;
        }
    }
    public void ResetEvent()
    {
        eventStarted = false;
        ResetBoolens();
    }
    public void StartEvent()
    {
        ResetBoolens();

        eventStarted = true;
        mainCircle.enabled = true;
        target.gameObject.SetActive(true);
        movingImage.gameObject.SetActive(true);
        pressButtonText.enabled = true;
        choseRandomSpeed = false;
    }
    void SetTargetPostion()
    {
        float targetRotaion = Random.Range(-360, 0);
        target.rectTransform.Rotate(0, 0, targetRotaion);
    }
    public void ResetBoolens()
    {
        triesIndex = tries;
        hitTheTarget = false;
        eventFailed = false;
        canBePressed = false;
    }
    void ChooseRandomSpeed()
    {
        if (!choseRandomSpeed)
        {
            rotaionSpeed = Random.Range(minRotaionSpeed, maxRotaionSpeed);
            choseRandomSpeed = true;
        }
    }
    void RotateMovingImage()
    {
        movingImage.rectTransform.Rotate(0, 0, -10 * rotaionSpeed *Time.deltaTime);
    }
    void CheckIfTargetIsTouchingMovingImage()
    {
        float rotationDifference = Quaternion.Angle(movingImage.rectTransform.rotation, target.rectTransform.rotation);
       //print("rotationDifference is : )" + rotationDifference);
        if (rotationDifference <= differenceBetweenTargetAndMovingImage)
        {
            canBePressed = true;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //print("E was pressed at the right time!");
                hitTheTarget = true;
                //audioSource.PlayOneShot(correctAudioClip); //replaced with pearl SFX
            }
        }
        else
        {
            canBePressed = false;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //print("E was NOT pressed at the right time!");
                if(triesIndex > 0)
                {
                    triesIndex--;
                    //print("You have " + triesIndex + " tries left");
                    audioSource.Stop();
                    audioSource.PlayOneShot(wrongAudioClip);
                    SetTargetPostion();
                }
                else
                {
                    eventFailed = true;
                    audioSource.Stop();
                    audioSource.PlayOneShot(wrongAudioClip);
                }

            }
        }

    }

}
