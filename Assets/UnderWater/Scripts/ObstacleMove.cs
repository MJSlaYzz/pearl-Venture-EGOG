using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform pos1; //point 1 of movement for obstacle
    [SerializeField] private Transform pos2; //point 2 of movement for obstacle

    [SerializeField] private PlayerHealth playerHealth;

    [HideInInspector] private bool shouldLerp;
    [HideInInspector] private float timer;

    [SerializeField] private bool shouldBeRandomized = true;
    [SerializeField] private float smoothTime = 5f; // the amount of time it takes for the obstacle to reach its destination.
    [SerializeField] private float goingOffSpeed = 15f;
    [SerializeField] private float goinBackSpeed = 5f;
    #endregion



    private void Start()
    {
        if (shouldBeRandomized)
        {
            smoothTime = Random.Range(5f, 10f);
            goingOffSpeed = Random.Range(15f, 25f);
            goinBackSpeed = Random.Range(5f, 7f);
        }
        transform.position = pos1.position;
    }
    // Update is called once per frame
    void Update()
    {
        LerpDown();
        LerpUp();
        //Debug.Log("timer" + timer);
    }

    void LerpDown()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (shouldLerp && timer >= 0)
        {
            timer += 1 * Time.deltaTime * goingOffSpeed;

            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            transform.position = Vector3.Lerp(pos1.transform.position, pos2.transform.position, inverseT);
        }
        if (transform.position == pos2.position)
        {
            shouldLerp = false;
        } 
    }
    void LerpUp()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (!shouldLerp && timer >= 0)
        {
            timer -= 1 * Time.deltaTime * goinBackSpeed;

            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            transform.position = Vector3.Lerp(pos1.transform.position, pos2.transform.position, inverseT);
        }
        if (transform.position == pos1.position)
        {
            shouldLerp = true;
        }
    }
    /*
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerHealth != null)
            {
                Debug.Log("Dead!");
                playerHealth.Dying();
            }
            else
            {
                Debug.Log("Player Health Script Not Found");
            }
        }
    }
    */
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerHealth != null)
            {
                Debug.Log("Dead!");
                playerHealth.Dying();
            }
            else
            {
                Debug.Log("Player Health Script Not Found");
            }
        }
    }
}

