using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] public Rigidbody2D fishRb;
    [HideInInspector] public Vector3 scale;
    [SerializeField] public Transform[] patrolPoints;
    [SerializeField] public float moveSpeed;
    [SerializeField] private bool simpleFlip = false;

    public int patrolDestination;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
        fishRb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (!simpleFlip)
        {
            //Vector3 direction = patrolPoints[index].position - transform.position;
            float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fishRb.rotation = angel;
            //Debug.Log(angel);
            if (angel > 0f && angel <= 90f)
            {
                transform.localScale = new Vector3(transform.localScale.x, scale.y, transform.localScale.z);
            }
            if (angel > 90f && angel <= 180)
            {
                transform.localScale = new Vector3(transform.localScale.x, -scale.y, transform.localScale.z);
            }
            if (angel > -180f && angel <= -90f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -scale.y, transform.localScale.z);
            }
            if (angel > -90f && angel <= 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x, scale.y, transform.localScale.z);
            }
        }
        else
        {
            if (direction.x > 0) // Moving right
            {
                transform.localScale = new Vector3(scale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < 0) // Moving left
            {
                transform.localScale = new Vector3(-scale.x, transform.localScale.y, transform.localScale.z);
            }
        }

    }
}
