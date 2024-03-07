using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabTrap : MonoBehaviour
{
    #region Variables
    [HideInInspector] private PlayerMovementFour playerMovement;
    [HideInInspector] public bool locked = true;
    [SerializeField] private bool isTouching = false;
    [SerializeField] private bool playedCutAnimation = false;
    [Header("UI variables")]
    [SerializeField] private GameObject eIcon;


    [SerializeField] GameObject crab;
    [SerializeField] GameObject lockedTrap;
    [SerializeField] GameObject unlockedTrap;
    [HideInInspector] private QuickTimeEventManager qTEManager;
    [HideInInspector] private QuickTimeEvent qTE;
    [HideInInspector] private Cop28DataManager cop28DataManager;
    [HideInInspector] private bool pointsAdded = false;

    [Header("Audio variables")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip cuttingSFX;


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        locked = true;
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
        //ControlTrap();
        PressButtonToStartUnlockEvent();
        CheckUnlocking();
        CutAnimation();
    }
    void CutAnimation()
    {
        if (!locked && isTouching && !playedCutAnimation)
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
    private IEnumerator ControlTrap()
    {
        //if (locked == true)
        //{
        //    lockedTrap.SetActive(true);
        //    unlockedTrap.SetActive(false);
        //    crab.SetActive(false);
        //}
        //else
        if(locked == false)
        {
            yield return new WaitForSeconds(0.5f);
            lockedTrap.SetActive(false);
            unlockedTrap.SetActive(true);
            crab.SetActive(true);
        }
    }
    void PressButtonToStartUnlockEvent()
    {
        if (locked && isTouching && !qTEManager.eventStarted)
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
            locked = false;
            StartCoroutine(ControlTrap());
            if (!pointsAdded)
            {
                cop28DataManager.AddPoints(20);
                pointsAdded = true;
                qTEManager.eventSuceeded = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerMovement.isCop28)
            {
                isTouching = true;
                if (locked)
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
