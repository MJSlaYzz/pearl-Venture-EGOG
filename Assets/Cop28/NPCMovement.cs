using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] public Rigidbody2D npcRb;
    [HideInInspector] public Vector3 scale;
    [SerializeField] public Transform[] patrolPoints;
    [SerializeField] public float moveSpeed = 1;
    private float timeBMA;
    private bool timeCountDown = false;
    private Vector3 movement;
    private Animator animator;

    public int patrolDestination;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
        npcRb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        //MoveToPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPatrolPoint();
        movement.x = npcRb.velocity.x;
        movement.y = npcRb.velocity.y;
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", moveSpeed);
        TimeBeforeMovingAgain();
    }

    void MoveToPatrolPoint()
    {
        if (!timeCountDown)
        {
            float directionX = patrolPoints[patrolDestination].position.x - npcRb.position.x;
            float directionY = patrolPoints[patrolDestination].position.y - npcRb.position.y;
            Vector2 direction = new Vector2(directionX, directionY);
            moveSpeed = 1;
            npcRb.velocity = direction.normalized * moveSpeed;

            if (patrolDestination == 0) //heading to point 0
            {
                if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.2f)
                {
                    moveSpeed = 0;
                    npcRb.velocity = new Vector2(0,0);
                    patrolDestination = 1;
                    timeBMA = Random.Range(2,7);
                    timeCountDown = true;
                }
            }
            if (patrolDestination == 1) //heading to point 1
            {
                if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.2f)
                {
                    moveSpeed = 0;
                    npcRb.velocity = new Vector2(0, 0);
                    patrolDestination = 0;
                    timeBMA = Random.Range(2, 7);
                    timeCountDown = true;
                }
            }
        }
    }
    void TimeBeforeMovingAgain()
    {
        //print(timeBMA);
        if (timeCountDown && timeBMA >= 0)
        {
            timeBMA -= Time.deltaTime;
        }
        if(timeBMA <= 0)
        {
            timeCountDown = false;
        }
    }
}
