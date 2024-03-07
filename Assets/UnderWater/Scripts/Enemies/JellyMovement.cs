using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyMovement : MonoBehaviour
{
    #region Variables
    // Collision 
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Patrol Attributes")]
    [SerializeField] public Transform[] patrolPoints;
    [SerializeField] public float moveSpeed;
    public int patrolDestination;
    [HideInInspector] public Rigidbody2D enemyRb;
    [HideInInspector] public bool isScouting = true;

    // Rotate 
    [HideInInspector] public Vector2 direction;
    
    //[HideInInspector] private AudioSource jellyfishAudio;
    
    #endregion

    void Start()
    {
        enemyRb = this.GetComponent<Rigidbody2D>();
        //jellyfishAudio = this.GetComponent<AudioSource>();
        
    }

    void Update()
    {
        direction.Normalize();
        MoveToPatrolPoint();
    }
    
    void MoveToPatrolPoint()
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
    
    void RotateOnPatrolPoint(int index)
    {
        Vector3 direction = patrolPoints[index].position - transform.position;
        float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemyRb.rotation = angel;
        //Debug.Log(angel);
        if (angel > 0f && angel <= 90f)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }
        if (angel > 90f && angel <= 180)
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
        }
        if (angel > -180f && angel <= -90f)
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
        }
        if (angel > -90f && angel <= 0f)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerRb != null)
            {
                //JellyFishAudio.Play();
            }
        }
    }


}
