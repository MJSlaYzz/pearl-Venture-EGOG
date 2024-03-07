using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    #region Variables

    [HideInInspector] private PlayerMovementFour PlayerMovementFour;
    [HideInInspector] private Vector3 mousePos;
    [HideInInspector] public bool isFighting = false;
    [SerializeField] private GameObject rightArrowPrefab;
    [SerializeField] private GameObject leftArrowPrefab;
    [SerializeField] private GameObject rightCrossbow;
    [SerializeField] private GameObject leftCrossbow;
    [SerializeField] private GameObject rightArrow;// only to show the arrow
    [SerializeField] private GameObject leftArrow; // only to show the arrow
    [SerializeField] private Transform arrowSpawnRight;
    [SerializeField] private Transform arrowSpawnLeft;
    [HideInInspector] private bool ammoAvailable = true;
    [HideInInspector] private bool reloadDone = true;
    [HideInInspector] private bool isShooting = false;
    [SerializeField] private GameObject crosshairRight;
    [SerializeField] private GameObject crosshairLeft;
    [SerializeField] public float reloadCooldown = 5f;
    [SerializeField] private float arrowSpeed = 5f;

    [HideInInspector] private Vector2 arrowDirection;

    [SerializeField] private bool shootingIsAllowed = false;
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

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovementFour = GetComponent<PlayerMovementFour>();
        if (shootingIsAllowed)
        {
            InstantiatePoints();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (shootingIsAllowed && instantiatedone)
        {
            Aiming();
            Crosshair();
            ShootingArrow();
        }
        else if(shootingIsAllowed && !instantiatedone)
        {
            InstantiatePoints();
        }
    }
    void Aiming()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            isFighting = true;
            if (PlayerMovementFour.isLookingRight)
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
            isFighting = false;
            rightCrossbow.SetActive(false);
            leftCrossbow.SetActive(false);
        }

        if (PlayerMovementFour.isLookingRight)
        {
            arrowDirection = crosshairRight.transform.position - rightCrossbow.transform.position;
            gravityScale = gravityScaleRight;
        }
        else
        {
            arrowDirection = crosshairLeft.transform.position - leftCrossbow.transform.position;
            gravityScale = gravityScaleLeft;
        }

        for (int i = 0; i < numberOfPoints; i++)
        {
            pointsRight[i].transform.position = PointPosition(i * spaceBetweenPoints, arrowSpawnRight);
            pointsLeft[i].transform.position = PointPosition(i * spaceBetweenPoints, arrowSpawnLeft);
            if (isFighting && PlayerMovementFour.isLookingRight)
            {
                pointsRight[i].SetActive(true);
                pointsLeft[i].SetActive(false);
            }
            else if (isFighting && !PlayerMovementFour.isLookingRight)
            {
                pointsRight[i].SetActive(false);
                pointsLeft[i].SetActive(true);
            }
            else
            {
                pointsRight[i].SetActive(false);
                pointsLeft[i].SetActive(false);
            }
        }


    }
    void ShootingArrow()
    {
        if (isFighting && ammoAvailable && reloadDone)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isShooting = true;
                if (PlayerMovementFour.isLookingRight)
                {

                    GameObject currenArrow = Instantiate(rightArrowPrefab, arrowSpawnRight.position, arrowSpawnRight.rotation);
                    currenArrow.transform.localScale = arrowSpawnRight.localScale;
                    rightArrow.SetActive(false);
                    arrowDirection = crosshairRight.transform.position - rightCrossbow.transform.position;
                    currenArrow.GetComponent<Rigidbody2D>().velocity = arrowDirection * arrowSpeed;
                    //rightArrow.SetActive(true);

                }
                else
                {
                    GameObject currenArrow = Instantiate(leftArrowPrefab, arrowSpawnLeft.position, arrowSpawnLeft.rotation);
                    currenArrow.transform.localScale = arrowSpawnLeft.localScale;
                    leftArrow.SetActive(false);
                    arrowDirection = crosshairLeft.transform.position - leftCrossbow.transform.position;
                    currenArrow.GetComponent<Rigidbody2D>().velocity = arrowDirection * arrowSpeed;
                    //leftArrow.SetActive(true);

                }
            }
            else
            {
                isShooting = false;
            }
        }
    }
    void Crosshair()
    {
        //mousePos = Input.mousePosition + new Vector3(0, -700, 0); // because the mouse is not centred.
        //print(mousePos);
        if (crosshairRight.activeInHierarchy)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp(mousePos.x, crosshairRight.transform.position.x + mouseMinPos, crosshairRight.transform.position.x + mouseMaxPos); //to avoid the glitch that happens when the mouse gets so close to the player.

            Vector2 rotation = mousePos - crosshairRight.transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            rotZ = Mathf.Clamp(rotZ, -30f, 30f);

            rightCrossbow.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }
        else if (crosshairLeft.activeInHierarchy)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp(mousePos.x, crosshairLeft.transform.position.x + mouseMinPos, crosshairLeft.transform.position.x + mouseMaxPos); //to avoid the glitch that happens when the mouse gets so close to the player.

            Vector2 rotation = mousePos - crosshairLeft.transform.position;
            float rotZ2 = Mathf.Atan2(-rotation.y, rotation.x) * Mathf.Rad2Deg;
            rotZ2 = Mathf.Clamp(rotZ2, -30f, 30f);

            leftCrossbow.transform.rotation = Quaternion.Euler(0, 0, rotZ2);

        }
    }
    Vector2 PointPosition(float t, Transform shotPoint)
    {
        Vector2 position = (Vector2)shotPoint.position + (arrowDirection.normalized * arrowSpeed * t) + 0.5f * (Physics2D.gravity/ gravityScale) * (t * t);
        return position;
    }
    void InstantiatePoints()
    {
        pointsRight = new GameObject[numberOfPoints];
        pointsLeft = new GameObject[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            pointsRight[i] = Instantiate(pointPrefab, rightCrossbow.transform.position, Quaternion.identity);
            pointsRight[i].SetActive(false);
        }
        for (int i = 0; i < numberOfPoints; i++)
        {
            pointsLeft[i] = Instantiate(pointPrefab, leftCrossbow.transform.position, Quaternion.identity);
            pointsLeft[i].SetActive(false);
        }
        instantiatedone = true;
    }
}
