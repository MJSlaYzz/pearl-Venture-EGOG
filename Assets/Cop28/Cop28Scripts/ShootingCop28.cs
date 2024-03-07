using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingCop28 : MonoBehaviour
{
    #region Variables

    [HideInInspector] private PlayerMovementFour playerMovementFour;
    [HideInInspector] private Vector3 mousePos;
    [HideInInspector] public bool isFighting = false;
    [SerializeField] private GameObject rightArrowPrefab;
    [SerializeField] private GameObject leftArrowPrefab;
    [SerializeField] private GameObject rightCrossbow;
    [SerializeField] private GameObject leftCrossbow;
    [SerializeField] private GameObject rightArrow;// only to show the arrow
    [SerializeField] private GameObject leftArrow; // only to show the arrow
    [SerializeField] public Transform arrowSpawnRight;
    [SerializeField] public Transform arrowSpawnLeft;
    [HideInInspector] private bool ammoAvailable = true;
    [HideInInspector] private bool reloadDone = true;
    [HideInInspector] private bool isShooting = false;
    [SerializeField] private GameObject crosshairRight;
    [SerializeField] private GameObject crosshairLeft;
    [SerializeField] public float reloadCooldown = 5f;
    [SerializeField] private float arrowSpeed = 5f;

    [HideInInspector] private Vector2 arrowDirection;

    [SerializeField] public bool shootingIsAllowed = false;
    [HideInInspector] private bool instantiatedone = false;

    [Header("Mouse Variables")]
    [SerializeField] private Camera cam;
    [SerializeField] private float mouseMinPos = 5f;
    [SerializeField] private float mouseMaxPos = 30f;

    [Header("Trajectory Variables")]
    [SerializeField] private GameObject pointPrefab;
    [HideInInspector] private GameObject[] pointsRight;
    [HideInInspector] private GameObject[] pointsLeft;
    [SerializeField] private float spaceBetweenPoints = 0.05f;
    [SerializeField] private int numberOfPoints = 10;
    [HideInInspector] private float gravityScale;
    [SerializeField] private float gravityScaleRight = 4.2f;
    [SerializeField] private float gravityScaleLeft = 4.5f;

    [Header("Cop28 Variables")]
    [HideInInspector] public bool arrowGotShot = false;
    [SerializeField] private float ropelength = 2f;
    [HideInInspector] public float disToPlayer;
    [SerializeField] float pullArrowSpeed = 2.5f;
    [SerializeField] private GameObject ArrowRightCop28;
    [SerializeField] private GameObject ArrowLeftCop28;
    [HideInInspector] private bool callArrowBack = false;
    [HideInInspector] public bool forceCallArrowBack = false;
    [SerializeField] private LineRenderer ropeRight;
    [SerializeField] private LineRenderer ropeLeft;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioClip shootAudio;
    [SerializeField] private AudioClip pullAudio;
    [SerializeField] private AudioClip forcePullAudio;
    [SerializeField] private ArrowCop28 arrowScriptRight;
    [SerializeField] private ArrowCop28 arrowScriptLeft;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        playerMovementFour = FindObjectOfType<PlayerMovementFour>();
        ropeRight.enabled = false;
        ropeLeft.enabled = false;

    }
    // Update is called once per frame
    void Update()
    {
        //mousePos = cam.ScreenToViewportPoint(Input.mousePosition);
        //print(mousePos);
        if (playerMovementFour.playerCanMove) // still testing (for QTE)
        {
            if (shootingIsAllowed && playerMovementFour.isCop28)  //&& instantiatedone)
            {
                Aiming();
                if (!arrowGotShot)
                {
                    Crosshair();
                }

                ShootingArrow();
            }
            if (Input.GetKeyDown(KeyCode.Q) && arrowGotShot && !forceCallArrowBack)
            {
                callArrowBack = true;
                audioSource.clip = pullAudio;
                audioSource.Play();
            }
        }

        FoceArrowBack();
        GetArrowBack();
    }
    void Aiming()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            isFighting = true;
            if (playerMovementFour.isLookingRight)
            {
                rightCrossbow.SetActive(true);
                leftCrossbow.SetActive(false);
            }
            else
            {
                rightCrossbow.SetActive(false);
                leftCrossbow.SetActive(true);
            }
        }
        else
        {
            if (!arrowGotShot) //prevents the animation change while the arrow is still not back
            {
                isFighting = false;
                rightCrossbow.SetActive(false);
                leftCrossbow.SetActive(false);
            }

        }

        if (playerMovementFour.isLookingRight)
        {

            arrowDirection = crosshairRight.transform.position - arrowSpawnRight.transform.position;
            gravityScale = gravityScaleRight;
        }
        else
        {

            arrowDirection = crosshairLeft.transform.position - arrowSpawnLeft.transform.position;
            gravityScale = gravityScaleLeft;
        }



    }
    void ShootingArrow()
    {
        //Debug.Log("arrowDirection is : " + arrowDirection);
        //Debug.DrawLine(PlayerMovementFour.transform.position, ArrowRightCop28.transform.position, Color.red);
        Debug.DrawLine(crosshairRight.transform.position, arrowSpawnRight.transform.position, Color.red);
        if (isFighting && ammoAvailable && reloadDone)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !arrowGotShot)
            {
                isShooting = true;
                audioSource.PlayOneShot(shootAudio);
                if (playerMovementFour.isLookingRight)
                {
                    ArrowRightCop28.SetActive(true);
                    ArrowRightCop28.transform.position = rightArrow.transform.position;
                    ArrowRightCop28.transform.rotation = rightArrow.transform.rotation;
                    ArrowRightCop28.transform.localScale = rightArrow.transform.localScale;
                    rightArrow.SetActive(false);
                    arrowDirection = crosshairRight.transform.position - arrowSpawnRight.transform.position;
                    ArrowRightCop28.GetComponent<Rigidbody2D>().velocity = arrowDirection * arrowSpeed;
                    arrowGotShot = true;
                    //Debug.Log("Arrow is shot to the right!");
                    ropeRight.enabled = true;

                }
                else
                {

                    ArrowLeftCop28.SetActive(true);
                    ArrowLeftCop28.transform.position = leftArrow.transform.position;
                    ArrowLeftCop28.transform.rotation = leftArrow.transform.rotation;
                    ArrowLeftCop28.transform.localScale = leftArrow.transform.localScale;
                    leftArrow.SetActive(false);
                    arrowDirection = crosshairLeft.transform.position - arrowSpawnLeft.transform.position;
                    ArrowLeftCop28.GetComponent<Rigidbody2D>().velocity = arrowDirection * arrowSpeed;
                    arrowGotShot = true;
                    //Debug.Log("Arrow is shot to the left!");
                    ropeLeft.enabled = true;

                }
            }
            else
            {
                if (!arrowGotShot)
                {
                    isShooting = false;
                }

            }
            if (arrowGotShot)
            {
                if (playerMovementFour.isLookingRight)
                {
                    disToPlayer = Vector2.Distance(ArrowRightCop28.transform.position, rightArrow.transform.position);
                }
                else
                {
                    disToPlayer = Vector2.Distance(ArrowLeftCop28.transform.position, leftArrow.transform.position);
                }
                //Debug.Log("Arrow disToPlayer is: " + disToPlayer);

            }
            if (disToPlayer >= ropelength || forceCallArrowBack)
            {
                //Debug.Log("Arrow disToPlayer is reached and it's = : " + disToPlayer);
                if (playerMovementFour.isLookingRight)
                {
                    arrowScriptRight.hasHit = true;
                    ArrowRightCop28.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    ArrowRightCop28.GetComponent<Rigidbody2D>().isKinematic = true;
                }
                else
                {
                    arrowScriptLeft.hasHit = true;
                    ArrowLeftCop28.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    ArrowLeftCop28.GetComponent<Rigidbody2D>().isKinematic = true;
                }

            }
        } 
    }
    void GetArrowBack()
    {
        if (callArrowBack && arrowGotShot)
        {
            //Debug.Log("Arrow Back Called!");
            if (playerMovementFour.isLookingRight)
            {
                if (disToPlayer >= 0.2)
                {
                    ArrowRightCop28.transform.position = Vector3.MoveTowards(ArrowRightCop28.transform.position, arrowSpawnRight.transform.position, pullArrowSpeed * Time.deltaTime);
                }
                else
                {
                    arrowScriptRight.hasHit = false;
                    ArrowRightCop28.transform.position = arrowSpawnRight.position;
                    ArrowRightCop28.transform.rotation = arrowSpawnRight.rotation;
                    ArrowRightCop28.transform.localScale = arrowSpawnRight.localScale;

                    ArrowRightCop28.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    ArrowRightCop28.GetComponent<Rigidbody2D>().isKinematic = false;
                    callArrowBack = false;
                    arrowGotShot = false;

                    //print("Test 001 Called!");
                    rightArrow.SetActive(true);
                    ArrowRightCop28.SetActive(false);
                    ropeRight.enabled = false;

                    audioSource.Stop();
                }
            }
            else
            {
                if (disToPlayer >= 0.2)
                {
                    ArrowLeftCop28.transform.position = Vector3.MoveTowards(ArrowLeftCop28.transform.position, arrowSpawnLeft.transform.position, pullArrowSpeed * Time.deltaTime);
                }
                else
                {
                    arrowScriptLeft.hasHit = false;
                    ArrowLeftCop28.transform.position = arrowSpawnLeft.position;
                    ArrowLeftCop28.transform.rotation = arrowSpawnLeft.rotation;
                    ArrowLeftCop28.transform.localScale = arrowSpawnLeft.localScale;

                    ArrowLeftCop28.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    ArrowLeftCop28.GetComponent<Rigidbody2D>().isKinematic = false;
                    callArrowBack = false;
                    arrowGotShot = false;

                    leftArrow.SetActive(true);
                    ArrowLeftCop28.SetActive(false);
                    ropeLeft.enabled = false;

                    audioSource.Stop();
                }
            }
        }  
    }
    void FoceArrowBack()
    {
        if (arrowGotShot && forceCallArrowBack)
        {
            if (!audioSource2.isPlaying)
            {
                audioSource2.PlayOneShot(forcePullAudio);
            }

            if (playerMovementFour.isLookingRight)
            {
                if (disToPlayer >= 0.2)
                {
                    ArrowRightCop28.transform.position = Vector3.MoveTowards(ArrowRightCop28.transform.position, arrowSpawnRight.transform.position, pullArrowSpeed * Time.deltaTime);
                }
                else
                {
                    ArrowRightCop28.transform.position = arrowSpawnRight.position;
                    ArrowRightCop28.transform.rotation = arrowSpawnRight.rotation;
                    ArrowRightCop28.transform.localScale = arrowSpawnRight.localScale;

                    ArrowRightCop28.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    ArrowRightCop28.GetComponent<Rigidbody2D>().isKinematic = false;
                    callArrowBack = false;
                    arrowGotShot = false;
                    forceCallArrowBack = false;

                    //print("Test 001 Called!");
                    rightArrow.SetActive(true);
                    ArrowRightCop28.SetActive(false);
                    ropeRight.enabled = false;

                    //audioSource.Stop();
                }
            }
            else
            {
                if (disToPlayer >= 0.2)
                {
                    ArrowLeftCop28.transform.position = Vector3.MoveTowards(ArrowLeftCop28.transform.position, arrowSpawnLeft.transform.position, pullArrowSpeed * Time.deltaTime);
                }
                else
                {
                    ArrowLeftCop28.transform.position = arrowSpawnLeft.position;
                    ArrowLeftCop28.transform.rotation = arrowSpawnLeft.rotation;
                    ArrowLeftCop28.transform.localScale = arrowSpawnLeft.localScale;

                    ArrowLeftCop28.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    ArrowLeftCop28.GetComponent<Rigidbody2D>().isKinematic = false;
                    callArrowBack = false;
                    arrowGotShot = false;
                    forceCallArrowBack = false;

                    leftArrow.SetActive(true);
                    ArrowLeftCop28.SetActive(false);
                    ropeLeft.enabled = false;

                    //audioSource.Stop();
                }
            }
        }
    }
    //void Crosshair()
    //{
    //    //mousePos = Input.mousePosition + new Vector3(0, -700, 0); // because the mouse is not centred.
    //    //print(mousePos);
    //    if (crosshairRight.activeInHierarchy)
    //    {
    //        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    //        mousePos.x = Mathf.Clamp(mousePos.x, crosshairRight.transform.position.x + mouseMinPos, crosshairRight.transform.position.x + mouseMaxPos); //to avoid the glitch that happens when the mouse gets so close to the player.

    //        Vector2 rotation = mousePos - crosshairRight.transform.position;
    //        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
    //        rotZ = Mathf.Clamp(rotZ, -30f, 30f);

    //        rightCrossbow.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    //    }
    //    else if (crosshairLeft.activeInHierarchy)
    //    {
    //        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    //        mousePos.x = Mathf.Clamp(mousePos.x, crosshairLeft.transform.position.x + mouseMinPos, crosshairLeft.transform.position.x + mouseMaxPos); //to avoid the glitch that happens when the mouse gets so close to the player.

    //        Vector2 rotation = mousePos - crosshairLeft.transform.position;
    //        float rotZ2 = Mathf.Atan2(-rotation.y, rotation.x) * Mathf.Rad2Deg;
    //        rotZ2 = Mathf.Clamp(rotZ2, -30f, 30f);

    //        leftCrossbow.transform.rotation = Quaternion.Euler(0, 0, rotZ2);

    //    }
    //}
    void Crosshair()
    {
        mousePos = cam.ScreenToViewportPoint(Input.mousePosition);

        if (crosshairRight.activeInHierarchy)
        {
            mousePos.x = Mathf.Clamp(mousePos.x, 0.6f + mouseMinPos, 0.6f + mouseMaxPos); //to avoid the glitch that happens when the mouse gets so close to the player.
            float playerToMouseAngle = Mathf.Atan2(mousePos.y - 0.5f, mousePos.x) * Mathf.Rad2Deg;
            playerToMouseAngle = Mathf.Clamp(playerToMouseAngle, -35f, 35f);
            rightCrossbow.transform.rotation = Quaternion.Euler(0, 0, playerToMouseAngle);
        }
        else if (crosshairLeft.activeInHierarchy)
        {
            mousePos.x = Mathf.Clamp(mousePos.x, 0.4f + mouseMinPos, 0.4f + mouseMaxPos); //to avoid the glitch that happens when the mouse gets so close to the player.
            float playerToMouseAngle = Mathf.Atan2(-mousePos.y + 0.5f, mousePos.x) * Mathf.Rad2Deg;
            playerToMouseAngle = Mathf.Clamp(playerToMouseAngle, -35f, 35f);
            leftCrossbow.transform.rotation = Quaternion.Euler(0, 0, playerToMouseAngle);
        }
    }
    public void ResetArrowWhenPlayerIsdead()
    {
        arrowGotShot = false;
        if (playerMovementFour.isLookingRight)
        {
            ArrowRightCop28.transform.position = arrowSpawnRight.transform.position;
        }
        else
        {
            ArrowLeftCop28.transform.position = arrowSpawnLeft.transform.position;
        }
        rightArrow.SetActive(true);
        ArrowRightCop28.SetActive(false);
        ropeRight.enabled = false;

        leftArrow.SetActive(true);
        ArrowLeftCop28.SetActive(false);
        ropeLeft.enabled = false;

    }
    void TestCal()
    {
        // right 0.6f on x
        // left 0.4f on x
        Vector3 worldPosition = crosshairLeft.transform.position;
        Vector3 viewportPosition = cam.WorldToViewportPoint(worldPosition);
        print(viewportPosition);
    }


}
