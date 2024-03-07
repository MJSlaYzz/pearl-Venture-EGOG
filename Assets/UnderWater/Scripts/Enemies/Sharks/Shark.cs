using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shark : MonoBehaviour
{
    #region Variables
    // Collision Variables
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Rigidbody2D playerRb;

    // Patrol Variables
    [SerializeField] public Transform[] patrolPoints;
    [SerializeField] public float moveSpeed;
    public int patrolDestination;
    [HideInInspector] public Rigidbody2D enemyRb;
    [HideInInspector] public bool isScouting = true;
    [HideInInspector] public Vector3 scale;

    // Chasing Variables
    [SerializeField] public Transform playerTransform;
    [HideInInspector] public bool isChasing = false;
    [HideInInspector] private float disToPlayer;
    [SerializeField] public float chaseDistance;

    // Rotate Variables
    [HideInInspector] public Vector2 direction;

    // Raycast Variables

    [SerializeField] private GameObject[] safeSpawnArea;
    [SerializeField] private bool inSpawnArea = false;

    [HideInInspector] private AudioSource sharkBiteAudio;
    
    #endregion

    void Start()
    {
        scale = transform.localScale;
        enemyRb = this.GetComponent<Rigidbody2D>();
        sharkBiteAudio = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        direction.Normalize();
        MoveToPatrolPoint();
        Chasing();
        CheckIfPlayerAlive();

        disToPlayer = Vector2.Distance(transform.position, playerTransform.position);
    }

    void CheckIfPlayerAlive()
    {
        if (playerHealth.playerIsDead == true)
        {
            isChasing = false;
            isScouting = true;
        }
    }

    void MoveToPatrolPoint()
    {
        if (isScouting == true)
        {
            if (patrolDestination == 0) //heading to point 0
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
                RotateOnPatrolPoint(0);
                if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.2f)
                {
                    //transform.localScale = new Vector3(scale.x, scale.y, scale.z); //looking left
                    patrolDestination = 1;
                }
            }
            if (patrolDestination == 1) //heading to point 1
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
                RotateOnPatrolPoint(1);
                if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.2f)
                {
                    //transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);// looking right
                    patrolDestination = 0;
                }
            }
        }
    }
    void Chasing()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, playerTransform.transform.position);
        //Debug.DrawLine(transform.position, hit.point,Color.red);

        if (hit.collider != null)
        {
            //Debug.Log("shark hit = " + hit.collider.tag);
        }

        if (disToPlayer < chaseDistance && !inSpawnArea)
        {
            isChasing = true;
            isScouting = false;

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Wall")
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
        }

        if (isChasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            RotateOnPlayer();
        }
    }

    void RotateOnPlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;
        float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemyRb.rotation = angel;
        //Debug.Log(angel);
        if (angel > 0f && angel <= 90f)
        {
            if(transform.localScale.y < 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
        if (angel > 90f && angel <= 180)
        {
            if (transform.localScale.y > 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
        if (angel > -180f && angel <= -90f)
        {
            if (transform.localScale.y > 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
        if (angel > -90f && angel <= 0f)
        {
            if (transform.localScale.y < 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
    }

    void RotateOnPatrolPoint(int index)
    {
        Vector3 direction = patrolPoints[index].position - transform.position;
        float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemyRb.rotation = angel;
        //Debug.Log(angel);
        if (angel > 0f && angel <= 90f)
        {
            if (transform.localScale.y < 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
        if (angel > 90f && angel <= 180)
        {
            if (transform.localScale.y > 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
        if (angel > -180f && angel <= -90f)
        {
            if (transform.localScale.y > 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
        if (angel > -90f && angel <= 0f)
        {
            if (transform.localScale.y < 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerRb != null)
            {
                sharkBiteAudio.Play();
                playerHealth.Dying();
                Debug.Log("player is dead");
                player.gameObject.SetActive(false);

            }
        }
    }
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (safeSpawnArea.Length == 2)
        {
            if (collision.gameObject == safeSpawnArea[0] || collision.gameObject == safeSpawnArea[1])
            {
                inSpawnArea = true;
                //print("Shark Entered safe zone");
            }
        }
        else if(safeSpawnArea.Length == 3)
        {
            if (collision.gameObject == safeSpawnArea[0] || collision.gameObject == safeSpawnArea[1] || collision.gameObject == safeSpawnArea[2])
            {
                inSpawnArea = true;
                //print("Shark Entered safe zone");
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            if (playerRb != null)
            {
                sharkBiteAudio.Play();
                playerHealth.Dying();
                //Debug.Log("player is dead");
                player.gameObject.SetActive(false);

            }
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (safeSpawnArea.Length == 2)
        {
            if (collision.gameObject == safeSpawnArea[0] || collision.gameObject == safeSpawnArea[1])
            {
                inSpawnArea = false;
                print("Shark Left safe zone");
            }
        }
        else if (safeSpawnArea.Length == 3)
        {
            if (collision.gameObject == safeSpawnArea[0] || collision.gameObject == safeSpawnArea[1] || collision.gameObject == safeSpawnArea[2])
            {
                inSpawnArea = false;
                print("Shark Left safe zone");
            }
        } 
    }

}
