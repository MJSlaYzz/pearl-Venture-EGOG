using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    [SerializeField] public GameObject player;
    [SerializeField] public PlayerHealth playerHealth;
    [SerializeField] public Enemy enemy;
    //[SerializeField] private Knockback knockback;
    [SerializeField] private Rigidbody2D playerRb;
    //[SerializeField] private float knockbackDuration = 0.2f;


    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(playerRb != null)
            {
                knockback.knockbackCounter = knockbackDuration;
                //playerMovement.knockbackCounter = playerMovement.knockbackTotalTime; //resets the counter.

                playerHealth.TakingDamage(damage);
                Debug.Log("player took damage Equals to: " + damage);

                if (playerHealth.currentHealth <= 0)
                {
                    Debug.Log("player is dead");
                    player.gameObject.SetActive(false);
                }
            }

        }
    }
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerRb != null)
            {
                //knockback.knockbackCounter = knockbackDuration;

                playerHealth.Dying();

                Debug.Log("player is dead");
                player.gameObject.SetActive(false);

            }
        }
    }
}
