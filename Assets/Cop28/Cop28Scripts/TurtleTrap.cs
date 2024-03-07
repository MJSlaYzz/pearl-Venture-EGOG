using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleTrap : MonoBehaviour
{
    #region Variables
    [HideInInspector] private PlayerMovementFour playerMovement;
    [HideInInspector] public bool trapped = true;
    [SerializeField] private bool isTouching = false;
    [SerializeField] private bool playedCutAnimation = false;
    [Header("UI variables")]
    [SerializeField] private GameObject eIcon;


    [SerializeField] private Animator animator;
    [SerializeField] private FishMovement FishMovementScript;
    [HideInInspector] private QuickTimeEventManager qTEManager;
    [HideInInspector] private QuickTimeEvent qTE;
    [HideInInspector] private Cop28DataManager cop28DataManager;
    [HideInInspector] private bool pointsAdded = false;

    [Header("Roaming Creature Variables")]
    [SerializeField] private bool isRoaming = false;

    [Header("Audio variables")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip cuttingSFX;


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        trapped = true;
        pointsAdded = false;
        playedCutAnimation = false;
        playerMovement = FindObjectOfType<PlayerMovementFour>();
        cop28DataManager = FindObjectOfType<Cop28DataManager>();
        qTEManager = FindObjectOfType<QuickTimeEventManager>();
        qTE = FindObjectOfType<QuickTimeEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("trapped", trapped);
        ControlMovement();
        PressButtonToStartUnlockEvent();
        CheckUnlocking();
        CutAnimation();
        RoamingCreature();
    }
    void CutAnimation()
    {
        if (!trapped && isTouching && !playedCutAnimation)
        {
            if (playerMovement.playerCanMove)
            {
                //Debug.Log("Player Cut Animation Called");
                eIcon.SetActive(false);
                audioSource.PlayOneShot(cuttingSFX);
                playerMovement.isCutting = true;
                playedCutAnimation = true;
            }
            else
            {
                //Debug.Log("Player can't move yet");
            }

        }
    }
    private void ControlMovement()
    {
        if (trapped == true)
        {
            FishMovementScript.enabled = false;
        }
        else if (trapped == false)
        {
            FishMovementScript.enabled = true;
        }
    }
    void PressButtonToStartUnlockEvent()
    {
        if (trapped && isTouching && !qTEManager.eventStarted)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                qTEManager.startEvent(qTE);
                eIcon.SetActive(false);
            }
        }
    }
    void CheckUnlocking()
    {
        if (qTEManager.eventSuceeded && isTouching)
        {
            trapped = false;
            if (!pointsAdded)
            {
                cop28DataManager.AddPoints(20);
                pointsAdded = true;
                qTEManager.eventSuceeded = false;
            }
        }
    }
    void RoamingCreature()
    {
        if (isRoaming)
        {
            trapped = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerMovement.isCop28)
            {
                isTouching = true;
                if (trapped)
                {
                    eIcon.SetActive(true);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerMovement.isCop28)
            {
                //print("Not Totcheddd!");
                isTouching = false;
            }
            eIcon.SetActive(false);
        }
    }
}
