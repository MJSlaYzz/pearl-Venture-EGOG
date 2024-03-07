using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Patrol Variables
    [SerializeField] public Transform[] patrolPoints;
    [SerializeField] public float moveSpeed;
    public int patrolDestination;
    [SerializeField] public Rigidbody2D enemyRb;
    [HideInInspector] public bool isScouting = true;
    [HideInInspector] public Vector3 scale;

    // Chasing Variables
    [SerializeField] public GameObject hunteyes;
    [SerializeField] public Transform playerTransform;
    [HideInInspector] public bool isChasing = false;
    public float chaseDistance;

    // Rotate Variables
    [HideInInspector] public Vector2 direction;
    //[HideInInspector] public float angle;

    [SerializeField] private PlayerHealth playerHealth;


    private void Start()
    {
        scale = transform.localScale;
        enemyRb = this.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        //position = transform.position;
        direction.Normalize();
        MoveToPatrolPoint();
        Chasing();
        CheckIfPlayerAlive();
    }

    private void CheckIfPlayerAlive()
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
        if (isChasing)
        {
            //Debug.Log("isChasing is ON");
            hunteyes.SetActive(true);

            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            RotateOnPlayer();
        }

        if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
        {
            isChasing = true;
            isScouting = false;
        }
        else
        {
            //Debug.Log("isChasing is OFF");
            hunteyes.SetActive(false);
            isChasing = false;
            isScouting = true;
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

    /*
    private void OldRotate()
    {
        //the moving was working fine but angle was off
        if (transform.position.x > playerTransform.position.x) // player is left
        {
            transform.localScale = new Vector3(scale.x, scale.y, scale.z);
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -1) * angle);
        }
        if (transform.position.x < playerTransform.position.x) // player is right
        {
            transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 1) * angle);
        }
        if (transform.position.y > playerTransform.position.y) // player is down
        {
            transform.localScale = new Vector3(scale.x, scale.y, scale.z);
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        }
        if (transform.position.y < playerTransform.position.y) //player up
        {
            transform.localScale = new Vector3(scale.x, scale.y * -1, scale.z);
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        }

    }
        */
}


