using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkCollisionController : MonoBehaviour
{
    #region Variables

    [HideInInspector] SharkAI sharkAI;

    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHealth playerHealth;

    [HideInInspector] private AudioSource sharkAudio;
    [SerializeField] private AudioClip sharkBite;

    [SerializeField] private GameObject[] safeSpawnArea;
    #endregion
    private void Start()
    {
        sharkAI = GetComponentInParent<SharkAI>();
        sharkAudio = GetComponentInParent<AudioSource>();
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerRb != null && !playerHealth.playerIsDead)
            {
                sharkBiteAudio.Play();
                playerHealth.Dying();
                //Debug.Log("player is dead");
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
                sharkAI.inSpawnArea = true;
                //print("print(sharkAI.inSpawnArea = );" + sharkAI.inSpawnArea);
                //print("Shark Entered safe zone");
            }
        }
        else if (safeSpawnArea.Length == 3)
        {
            if (collision.gameObject == safeSpawnArea[0] || collision.gameObject == safeSpawnArea[1] || collision.gameObject == safeSpawnArea[2])
            {
                sharkAI.inSpawnArea = true;
               //print("print(sharkAI.inSpawnArea = );" +sharkAI.inSpawnArea);
                //print("Shark Entered safe zone");
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("dead is called");
            if (playerRb != null && !playerHealth.playerIsDead)
            {
                sharkAudio.PlayOneShot(sharkBite);
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
                sharkAI.inSpawnArea = false;
                //print("print(sharkAI.inSpawnArea = );" + sharkAI.inSpawnArea);
                //print("Shark Left safe zone");
            }
        }
        else if (safeSpawnArea.Length == 3)
        {
            if (collision.gameObject == safeSpawnArea[0] || collision.gameObject == safeSpawnArea[1] || collision.gameObject == safeSpawnArea[2])
            {
                sharkAI.inSpawnArea = false;
                //print("print(sharkAI.inSpawnArea = );" + sharkAI.inSpawnArea);
                //print("Shark Left safe zone");
            }
        }
    }
}
