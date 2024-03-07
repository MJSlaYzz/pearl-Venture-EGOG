using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static UnityEngine.GraphicsBuffer;
using System.Reflection;

public class SharkAI : MonoBehaviour
{
    #region Variables
   
    // Collision Variables
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHealth playerHealth;

    // Patrol Variables
    [SerializeField] public Transform[] patrolPoints;
    [SerializeField] public int patrolDestination;
    [HideInInspector] public Rigidbody2D enemyRb;
    [HideInInspector] public bool isScouting = true;
    [HideInInspector] public Vector3 scale;

    // Chasing Variables
    [SerializeField] public Transform playerTransform;
    [HideInInspector] public bool isChasing = false;
    [HideInInspector] public float disToPlayer;
    [SerializeField] public float chaseDistance = 4.5f;

    // Rotate Variables
    [HideInInspector] public Vector2 direction;
    [SerializeField] private Transform enemySprite;

    // Raycast Variables
    [SerializeField] public bool inSpawnArea = false;

    // Audio and effects Variables
    [HideInInspector] private AudioSource sharkAudio;
    [SerializeField] private AudioClip sharkAlert;
    [HideInInspector] private bool alertAudioPlayed = false;
    [SerializeField] private GameObject exclamationMark;

    // AI Pathfinding Variables

    [HideInInspector] private float pathCalculationRate = 1.5f;

    [SerializeField] private float pathCalculationPlayer = 0.1f;
    [SerializeField] private float pathCalculationPoint = 0.5f;
    [SerializeField] private float distanceBeforeSwitchPoints = 2f;

    [HideInInspector] private Path path;
    [HideInInspector] int currentWaypoint = 0;
    [HideInInspector] Seeker seeker;
    [HideInInspector] private Transform target;
    [HideInInspector] private float nextWaypointDestance = 1f;
    [SerializeField] private float nextWaypointDestancPoint = 2f;

    [SerializeField] AIPath aiPath;
    [HideInInspector] bool pathCanUpdate = true;

    [Header("Shark Speed")]
    [SerializeField] public float sharkMinSpeed = 4f;
    [SerializeField] public float sharkMaxcSpeed = 4.5f;
    [Header("Cop28 Variables")]
    [SerializeField] public bool cop28IsActive = false;
    [SerializeField] public bool playerGotStuck = false;
    [SerializeField] private float StuckPlayerChaseSpeed = 3f;
    [SerializeField] public bool sharkShouldHide = true;
    [HideInInspector] private float randomChoosenSpeed;



    #endregion

    void Start()
    {
        scale = transform.localScale;
        enemyRb = this.GetComponent<Rigidbody2D>();
        sharkAudio = this.GetComponent<AudioSource>();
        seeker = GetComponent<Seeker>();
        aiPath.maxSpeed = Random.Range(sharkMinSpeed, sharkMaxcSpeed);
        randomChoosenSpeed = aiPath.maxSpeed;
    }
    void Update()
    {
        direction.Normalize();
        MoveToPatrolPoint();
        Chasing();
        PathCalculationCheck();

        disToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (pathCanUpdate)
        {
            StartCoroutine(UpdatePath());
        }

    }
    IEnumerator UpdatePath()
    {
        //print("Path Update");
        pathCanUpdate = false;
        if (seeker.IsDone())
        {
            seeker.StartPath(enemyRb.position, target.position, OnPathComplete);
        }
        yield return new WaitForSeconds(pathCalculationRate);
        pathCanUpdate = true;
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void FixedUpdate()
    {
        PathFinder();
        Rotate();
    }
    void PathFinder()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            pathCalculationRate = 0.1f; // avoid the issue where the shark stay for 1 sec after reacting his target.
            return;
        }
        else
        {
            float distance = Vector2.Distance(enemyRb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDestance)
            {
                currentWaypoint++;

            }
        }
    }
    void MoveToPatrolPoint()
    {
        if (isScouting == true)
        {
            if (patrolDestination == 0) //heading to point 0
            {
                target = patrolPoints[0].transform;

                if (Vector2.Distance(transform.position, patrolPoints[0].position) <= nextWaypointDestancPoint + distanceBeforeSwitchPoints)
                {
                    patrolDestination = 1;
                }
            }
            if (patrolDestination == 1) //heading to point 1
            {
                target = patrolPoints[1].transform;

                if (Vector2.Distance(transform.position, patrolPoints[1].position) <= nextWaypointDestancPoint + distanceBeforeSwitchPoints)
                {
                    patrolDestination = 0;
                }
            }
            alertAudioPlayed = false;
            if (exclamationMark != null && exclamationMark.activeInHierarchy)
            {
                exclamationMark.SetActive(false);
            }
        }
    }
    void Chasing()
    {
        if (cop28IsActive)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, playerTransform.transform.position);
            if (playerGotStuck)
            {
                aiPath.maxSpeed = StuckPlayerChaseSpeed;
                isChasing = true;
                isScouting = false;
            }
            else
            {
                aiPath.maxSpeed = randomChoosenSpeed;
                if (disToPlayer < chaseDistance && !inSpawnArea)
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.tag == "Player")
                        {
                            isChasing = true;
                            isScouting = false;
                        }
                        else if (hit.collider.tag == "Wall")
                        {
                            isChasing = false;
                            isScouting = true;
                            nextWaypointDestance = 1f;
                        }
                        else
                        {
                            isChasing = false;
                            isScouting = true;
                        }
                    }
                }
                else
                {
                    isChasing = false;
                    isScouting = true;
                    nextWaypointDestance = 1f;
                }
            }
            if (isChasing)
            {
                target = playerTransform.transform;
                nextWaypointDestance = 1f;
                if (!alertAudioPlayed && !playerHealth.playerIsDead)
                {
                    sharkAudio.PlayOneShot(sharkAlert);
                    alertAudioPlayed = true;
                }
                if (exclamationMark != null && !exclamationMark.activeInHierarchy)
                {
                    exclamationMark.SetActive(true);
                }
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, playerTransform.transform.position);
            if (disToPlayer < chaseDistance && !inSpawnArea)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Player")
                    {
                        isChasing = true;
                        isScouting = false;
                    }
                    else if (hit.collider.tag == "Wall")
                    {
                        isChasing = false;
                        isScouting = true;
                        nextWaypointDestance = 1f;
                    }
                    else
                    {
                        isChasing = false;
                        isScouting = true;
                    }
                }
            }
            else
            {
                isChasing = false;
                isScouting = true;
                nextWaypointDestance = 1f;
            }

            if (isChasing)
            {
                target = playerTransform.transform;
                nextWaypointDestance = 1f;
                if (!alertAudioPlayed && !playerHealth.playerIsDead)
                {
                    sharkAudio.PlayOneShot(sharkAlert);
                    alertAudioPlayed = true;
                }

                if (exclamationMark != null && !exclamationMark.activeInHierarchy)
                {
                    exclamationMark.SetActive(true);
                }
            }
        }
    }
    void PathCalculationCheck()
    {
        if(isScouting) 
        {
            if (pathCalculationRate != pathCalculationPoint) // avoid the issue of pathCalculationRate value not changing in the middle of the Coroutine methode.
            {
                StopCoroutine(UpdatePath());
                pathCalculationRate = pathCalculationPoint;
                StartCoroutine(UpdatePath());
            }
        }
        else if (isChasing)
        {
            if (pathCalculationRate != pathCalculationPlayer) // avoid the issue of pathCalculationRate value not changing in the middle of the Coroutine methode.
            {
                StopCoroutine(UpdatePath());
                pathCalculationRate = pathCalculationPlayer;
                StartCoroutine(UpdatePath());
            }
        }
    }
    void Rotate()
    {
        //print(aiPath.rotation);
        Vector3 newScale1 = new Vector3(enemySprite.localScale.x, 1.5f, enemySprite.localScale.z);
        Vector3 newScale2 = new Vector3(enemySprite.localScale.x, -1.5f, enemySprite.localScale.z);

        if (aiPath.rotation.z <= -0.01)
        {
            if(aiPath.rotation.w >= 0.01)
            {
                enemySprite.localScale = newScale1;
            }
            else if(aiPath.rotation.w <= -0.01)
            {
                enemySprite.localScale = newScale2;
            }
        }
        else if(aiPath.rotation.z >= 0.01)
        {
            if (aiPath.rotation.w >= 0.01)
            {
                enemySprite.localScale = newScale2;
            }
            else if (aiPath.rotation.w <= -0.01)
            {
                enemySprite.localScale = newScale1;
            }

        }
    }
    public void TurnOffPlayerGotStuck()
    {
        playerGotStuck = false;
        //print("Turn off player got stuck!");
        isChasing = false;
        isScouting = true;
        nextWaypointDestance = 1f;
    }
}
