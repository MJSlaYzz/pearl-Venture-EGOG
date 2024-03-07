using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static UnityEngine.GraphicsBuffer;
using System.Reflection;

public class OldSharkAI : MonoBehaviour
{
    #region Variables

    // Collision Variables
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHealth playerHealth;
    //[SerializeField] private Rigidbody2D playerRb;

    // Patrol Variables
    [SerializeField] public Transform[] patrolPoints;
    //[SerializeField] public float moveSpeed = 4f;
    [SerializeField] public int patrolDestination;
    [HideInInspector] public Rigidbody2D enemyRb;
    [HideInInspector] public bool isScouting = true;
    [HideInInspector] public Vector3 scale;

    // Chasing Variables
    [SerializeField] public Transform playerTransform;
    [HideInInspector] public bool isChasing = false;
    [HideInInspector] private float disToPlayer;
    [SerializeField] public float chaseDistance = 4.5f;

    // Rotate Variables
    [HideInInspector] public Vector2 direction;
    [SerializeField] private Transform enemySprite;

    // Raycast Variables

    //[SerializeField] public GameObject[] safeSpawnArea;
    [SerializeField] public bool inSpawnArea = false;

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
    //[HideInInspector] bool reachedEndOfPath = false;
    [HideInInspector] Seeker seeker;
    [HideInInspector] private Transform target;
    //[SerializeField] float chaseSpeed = 4.5f;
    [HideInInspector] private float nextWaypointDestance = 1f;
    [SerializeField] private float nextWaypointDestancPoint = 2f;
    //[SerializeField] private float nextWaypointDestancePlayer = 0.9f;


    [SerializeField] AIPath aiPath;
    [HideInInspector] bool pathCanUpdate = true;
    #endregion

    void Start()
    {
        scale = transform.localScale;
        enemyRb = this.GetComponent<Rigidbody2D>();
        sharkAudio = this.GetComponent<AudioSource>();
        seeker = GetComponent<Seeker>();
        //InvokeRepeating("UpdatePath", 0f, pathCalculationRate);
    }
    void Update()
    {
        direction.Normalize();
        MoveToPatrolPoint();
        Chasing();
        CheckIfPlayerAlive();
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
        //seeker.StartPath(enemyRb.position, target.position, OnPathComplete);
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
        //Debug.Log(pathCalculationRate);
    }
    void PathFinder()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            pathCalculationRate = 0.1f; // avoid the issue where the shark stay for 1 sec after reacting his target.
            //reachedEndOfPath = true;
            return;
        }
        else
        {
            float distance = Vector2.Distance(enemyRb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDestance)
            {
                currentWaypoint++;

            }
            /*
            //reachedEndOfPath = false;
            Vector3 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRb.position).normalized; //changed from Vector2 to Vector3.
            Vector2 force = direction * moveSpeed * Time.deltaTime;
            if(isChasing) 
            {
                
                if(pathCalculationRate != pathCalculationPlayer) // avoid the issue of pathCalculationRate value not changing in the middle of the Coroutine methode.
                {
                    StopCoroutine(UpdatePath());
                    pathCalculationRate = pathCalculationPlayer;
                    StartCoroutine(UpdatePath());
                }
                
                //aiPath.maxSpeed = chaseSpeed;
                //transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, chaseSpeed * Time.deltaTime); //testing
            }
            else if (isScouting)
            {
                
                if (pathCalculationRate != pathCalculationPoint) // avoid the issue of pathCalculationRate value not changing in the middle of the Coroutine methode.
                {
                    StopCoroutine(UpdatePath());
                    pathCalculationRate = pathCalculationPoint;
                    StartCoroutine(UpdatePath());
                }
                
                //aiPath.maxSpeed = 0.001f;
                //transform.Translate(force, Space.World);
                //transform.Translate(direction * moveSpeed * Time.smoothDeltaTime, Space.World);
                //transform.position += direction * Time.deltaTime * moveSpeed;
                //pathCalculationRate = pathCalculationPoint;
            }
            */ //movement was removed from this script cause the shark movement was being affected by two scripts which caused a Movement Stutter.
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
                    /*
                    StopCoroutine(UpdatePath());
                    pathCalculationRate = pathCalculationPoint;
                    StartCoroutine(UpdatePath());
                    */
                    patrolDestination = 1;
                }
            }
            if (patrolDestination == 1) //heading to point 1
            {
                target = patrolPoints[1].transform;

                if (Vector2.Distance(transform.position, patrolPoints[1].position) <= nextWaypointDestancPoint + distanceBeforeSwitchPoints)
                {
                    /*
                    StopCoroutine(UpdatePath());
                    pathCalculationRate = pathCalculationPoint;
                    StartCoroutine(UpdatePath());
                    */

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
        RaycastHit2D hit = Physics2D.Linecast(transform.position, playerTransform.transform.position);
        //Debug.DrawLine(transform.position, hit.point, Color.red);
        //Debug.Log(hit.collider.tag);
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
                    //nextWaypointDestance = nextWaypointDestancPoint;
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
            //nextWaypointDestance = nextWaypointDestancPoint;
            nextWaypointDestance = 1f;
        }

        if (isChasing)
        {
            target = playerTransform.transform;
            //nextWaypointDestance = nextWaypointDestancePlayer;
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
            //transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            //RotateOnPlayer();
        }
    }
    void PathCalculationCheck()
    {
        if (isScouting)
        {
            if (pathCalculationRate != pathCalculationPoint) // avoid the issue of pathCalculationRate value not changing in the middle of the Coroutine methode.
            {
                StopCoroutine(UpdatePath());
                pathCalculationRate = pathCalculationPoint;
                StartCoroutine(UpdatePath());
                //Debug.Log("Path Rate Fixed 1");
            }
        }
        else if (isChasing)
        {
            if (pathCalculationRate != pathCalculationPlayer) // avoid the issue of pathCalculationRate value not changing in the middle of the Coroutine methode.
            {
                StopCoroutine(UpdatePath());
                pathCalculationRate = pathCalculationPlayer;
                StartCoroutine(UpdatePath());
                //Debug.Log("Path Rate Fixed 1");
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
            if (aiPath.rotation.w >= 0.01)
            {
                enemySprite.localScale = newScale1;
            }
            else if (aiPath.rotation.w <= -0.01)
            {
                enemySprite.localScale = newScale2;
            }
        }
        else if (aiPath.rotation.z >= 0.01)
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
    void CheckIfPlayerAlive()
    {
        if (playerHealth.playerIsDead == true)
        {
            isChasing = false;
            isScouting = true;
            //nextWaypointDestance = nextWaypointDestancPoint;
            nextWaypointDestance = 1f;
        }
    }
}
