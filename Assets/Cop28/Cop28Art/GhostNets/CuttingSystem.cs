using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingSystem : MonoBehaviour
{
    #region Variables
    [HideInInspector] private  PlayerMovementFour playerMovement;
    [HideInInspector] private PlayerHealth playerHealth;
    [HideInInspector] private Cop28DataManager cop28DataManager;
    [SerializeField] private bool readyToCut;
    [SerializeField] private bool isTouching;
    [HideInInspector] private bool gotCut = false;
    [SerializeField] public bool onCooldown = false;
    [Header("Stuck Variables")]
    [SerializeField] private int chanceOfGettingStuck = 3;
    [SerializeField] private bool canGetYouStuck = true;
    [Header("UI variables")]
    [SerializeField] private GameObject eIcon;
    [Header("Shark variables")]
    [SerializeField] private SharkAI nearSharkScript;
    [SerializeField] float distanceBeforeSharkHide = 20f;
    [SerializeField] float cooldownForSharkBeforeHide = 10f;
    [HideInInspector] bool sharkCanHide = false;
    [SerializeField] bool gotStuck = false;
    [SerializeField] bool canCutNormally = false; //made cause sometimes it get cut and the QTE started
    [HideInInspector] bool sharkHideCooldownStart = false;
    [HideInInspector] float temCooldownForSharkBeforeHide;
    [SerializeField] Vector3 sharkStartingPoint;
    [HideInInspector] bool chanceToGetStuckCDStart = false;
    [HideInInspector] private float chanceToStuckCD = 0.2f;
    [HideInInspector] private float temChanceToStuckCD;

    [HideInInspector] private QuickTimeEvent2 qTE2;
    [HideInInspector] private Animator animator;
    [Header("Audio variables")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip cuttingSFX;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementFour>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        cop28DataManager = FindObjectOfType<Cop28DataManager>();
        qTE2 = FindObjectOfType<QuickTimeEvent2>();
        animator = GetComponent<Animator>();
        eIcon.SetActive(false);
        sharkStartingPoint = nearSharkScript.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //print(nearSharkScript.disToPlayer);
        animator.SetBool("GotCut", gotCut);
        onCooldown = qTE2.onCooldown;
        if (playerHealth.playerKnockedDown)
        {
            //print("Looks like player is down");
            qTE2.onCooldown = false;
            //readyToCut = true;
        }
        if(isTouching && qTE2.readyToCut)
        {
            readyToCut = true;
        }
        else if(!isTouching)
        {
            gotStuck = false;
        }
        PressButtonToCut();
        CutNetAfterStuck();
        HidesSharkAfterTime();
        SharkHideCooldown();
        ChanceToGetStuckCD();
        //print(temChanceToStuckCD);

    }
    void CutNetAfterStuck()
    {
        //print("qTE2.eventSucceeded = " + qTE2.eventSucceeded);
        if (qTE2.eventSucceeded && gotStuck && isTouching)
        {
            gotStuck = false;
            transform.GetComponent<PolygonCollider2D>().enabled = false;
            eIcon.SetActive(false);
            audioSource.PlayOneShot(cuttingSFX);
            playerMovement.isCutting = true;
            this.gotCut = true;
            cop28DataManager.AddPoints(10); // Add Points
        }
    }
    void PressButtonToCut()
    {
        if (readyToCut && !gotCut && isTouching && canCutNormally)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.GetComponent<PolygonCollider2D>().enabled = false;
                eIcon.SetActive(false);
                audioSource.PlayOneShot(cuttingSFX);
                playerMovement.isCutting = true;
                this.gotCut = true;
                cop28DataManager.AddPoints(10); // Add Points
            }
        }
    }
    void ChanceToGetStuckCD()
    {
        if (chanceToGetStuckCDStart)
        {
            if (temChanceToStuckCD > 0)
            {
                //print("sharkHide started");
                temChanceToStuckCD -= Time.deltaTime;
            }
            else
            {
                //print("sharkHide ended");
                ChanceToGetStuck();
                chanceToGetStuckCDStart = false;
                //temChanceToStuckCD = chanceToStuckCD;
            }
        }
    }
    void ChanceToGetStuck()
    {
        if (!onCooldown && canGetYouStuck)
        {
            int randomNum = Random.Range(0, chanceOfGettingStuck);
            if (randomNum == 0)
            {
                gotStuck = true;
                onCooldown = true;
                readyToCut = false;
                //print("Got Stuck!");
                if (!qTE2.eventStarted)
                {
                    qTE2.StartEvent();
                }
                eIcon.SetActive(false);
                StartSharkAttack();
            }
            else
            {
                canCutNormally = true;
            }
        }
    }
    void StartSharkAttack()
    {
        if(nearSharkScript.disToPlayer > 10) //to avoid the shark disapearing when near to player.
        {
            nearSharkScript.transform.position = sharkStartingPoint;
        }
        nearSharkScript.transform.parent.gameObject.SetActive(true);
        nearSharkScript.cop28IsActive = true;
        nearSharkScript.playerGotStuck = true;
        temCooldownForSharkBeforeHide = cooldownForSharkBeforeHide;
        sharkHideCooldownStart = true;
    }
    void SharkHideCooldown() // temCooldownForSharkBeforeHide should = cooldownForSharkBeforeHide && sharkHideCooldownStart should = true;
    {
        if (!sharkCanHide && sharkHideCooldownStart && nearSharkScript.sharkShouldHide)
        {
            if (temCooldownForSharkBeforeHide >= 0)
            {
                //print("sharkHide started");
                temCooldownForSharkBeforeHide -= Time.deltaTime;
            }
            else
            {
                //print("sharkHide ended");
                sharkHideCooldownStart = false;
                sharkCanHide = true;
            }
        }
    }
    void HidesSharkAfterTime()
    {
        if (nearSharkScript.isActiveAndEnabled && nearSharkScript.disToPlayer >= distanceBeforeSharkHide && sharkCanHide)
        {
            //print("SHARK WILL HIDE");
            nearSharkScript.transform.parent.gameObject.SetActive(false);
            sharkCanHide = false;
        }

    }
    //public void PlayerUnStuck()
    //{
    //    if(NearSharkScript.playerGotStuck == true)
    //    {
    //        //print("Got Unstuck!");
    //        NearSharkScript.playerGotStuck = false;
    //    }
    //} 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerMovement.isCop28)
            {
                readyToCut = true;
                isTouching = true;
                if (!gotCut && canGetYouStuck)
                {
                    eIcon.SetActive(true);
                    // player will get stuck every time he dashs into the net.
                    if (playerMovement.rb.velocity.x > playerMovement.moveSpeed || playerMovement.rb.velocity.y > playerMovement.moveSpeed || playerMovement.dashPlaying)
                    {
                        //print("Player Dashing in");
                        gotStuck = true;
                        onCooldown = true;
                        readyToCut = false;
                        if (!qTE2.eventStarted)
                        {
                            qTE2.StartEvent();
                        }
                        StartSharkAttack();
                        eIcon.SetActive(false);
                    }
                    else
                    {
                        //ChanceToGetStuck();
                        temChanceToStuckCD = chanceToStuckCD;
                        chanceToGetStuckCDStart = true;
                        eIcon.SetActive(true);
                    }
                }
                //if (readyToCut && !gotCut)
                //{
                //    if (Input.GetKeyDown(KeyCode.E))
                //    {
                //        transform.GetComponent<PolygonCollider2D>().enabled = false;
                //        eIcon.SetActive(false);
                //        audioSource.PlayOneShot(cuttingSFX);
                //        playerMovement.isCutting = true;
                //        this.gotCut = true;
                //    }
                //}
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
                readyToCut = false;
                isTouching = false;
                canCutNormally = false;
            }
            eIcon.SetActive(false);
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (readyToCut && !gotCut && isTouching)
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            transform.GetComponent<PolygonCollider2D>().enabled = false;
    //            eIcon.SetActive(false);
    //            audioSource.PlayOneShot(cuttingSFX);
    //            playerMovement.isCutting = true;
    //            this.gotCut = true;
    //        }
    //    }
    //}
}
