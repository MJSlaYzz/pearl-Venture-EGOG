using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float knockbackForce;     // how hard our player will be knockbacked. better to be higher than 4
    [HideInInspector] public float knockbackCounter;   // how much time is left on the knockback effect.
    [HideInInspector] public float knockbackTotalTime; // how long will the knockback effect will last altogether.
    [SerializeField] private Transform enemyTransform;

    [SerializeField] private PlayerMovement playerMovement;

    private bool isTouchingTheWall = false;


    // Update is called once per frame
    void Update()
    {
        if (knockbackCounter > 0) // you can only move if the counter is done
        {
            if (!isTouchingTheWall)
            {
                playerMovement.enabled = false;
                KnockBack();
            }
        }
        else if (knockbackCounter <= 0)
        {
            knockbackCounter = knockbackTotalTime;
            playerMovement.enabled = true;
        }
    }
    public void KnockBack()
    {
        Vector3 direction = (transform.position - enemyTransform.position);
        direction.Normalize();
        transform.Translate(direction * knockbackForce * Time.deltaTime);
        //transform.rotation = Quaternion.LookRotation(Vector3.zero);
        knockbackCounter -= Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            isTouchingTheWall = true;
            Debug.Log("Playe is touching a wall");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isTouchingTheWall = false;
            Debug.Log("Playe no more touching a wall");
        }
    }
}
