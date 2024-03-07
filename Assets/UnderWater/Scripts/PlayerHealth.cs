using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector] public int maxHealth = 6;
    [HideInInspector] public int currentHealth;
    [SerializeField] public bool playerIsDead = false;
    [SerializeField] private GameObject player;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private Transform checkPoint2;

    [HideInInspector] private bool spawnPointIsReady = false;
    [HideInInspector] private bool spawnPointIsReady2 = false;
    [SerializeField] private AudioSource spawnPointAudio;

    [SerializeField] private CameraMovement cameraMovement;
    //[SerializeField] private float delayBeforeSpawn = 2.5f;

    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject youDiedScreen;

    [SerializeField] private OxygenSystem oxygenSystem;
    [HideInInspector] private PlayerMovementFour playerMovementFour;
    [HideInInspector] private ShootingCop28 shootingCop28;

    [SerializeField] private AudioSource playerDeathAudio;
    [SerializeField] private AudioSource playerHeartbeatsAudio;

    [HideInInspector] private CheckpointHandler checkpoint1;
    [HideInInspector] private CheckpointHandler checkpoint2;

    [HideInInspector] private QuickTimeEventManager qTE1;
    [HideInInspector] private QuickTimeEvent2 qTE2;
    [HideInInspector] private QuickTimeEvent3 qTE3;

    [SerializeField] public bool playerKnockedDown = false; // for cop28

    private void Start()
    {
        playerMovementFour = GetComponent<PlayerMovementFour>();
        shootingCop28 = FindObjectOfType<ShootingCop28>();
        currentHealth = maxHealth;
        //Debug.Log("Player Health = " + currentHealth);
        qTE1 = FindObjectOfType<QuickTimeEventManager>();
        qTE2 = FindObjectOfType<QuickTimeEvent2>();
        qTE3 = FindObjectOfType<QuickTimeEvent3>();
    }
    public void Dying()
    {
        playerKnockedDown = true; // for cop28
        if (!playerIsDead)
        {
            playerHeartbeatsAudio.gameObject.SetActive(false); // player heartbeat SFX
            playerDeathAudio.Play();
            currentHealth = currentHealth - 1;
            //Debug.Log("Player Health = " + currentHealth);
            GetComponent<PlayerMovementFour>().enabled = false;
            playerIsDead = true;
            cameraMovement.followPlayer = false;
            if (currentHealth > 0)
            {
                inGameUI.SetActive(false);
                youDiedScreen.SetActive(true);
                //Invoke("Spawn", delayBeforeSpawn);
                Spawn();
                Time.timeScale = 0f;
            }
            else if (currentHealth <= 0)
            {
                GameOver();
            }
        }
    }
    public void Spawn()
    {
        //playerKnockedDown = false; // for cop28
        playerMovementFour.playerCanMove = true;
        playerHeartbeatsAudio.gameObject.SetActive(true); // player heartbeat SFX
        //Debug.Log("You are Dead, Back to spawn point");
        playerIsDead = false;
        if (spawnPointIsReady)
        {
            player.transform.position = checkPoint.position;
        }
        else if (checkPoint2 != null && spawnPointIsReady2)
        {
            player.transform.position = checkPoint2.position;
        }
        else
        {
            player.transform.position = spawnPoint.position;
        }

        if (player.activeInHierarchy == false)
        {
            player.SetActive(true);
        }
        GetComponent<PlayerMovementFour>().enabled = true;
        oxygenSystem.InstantFullOxygen();
        playerMovementFour.ResetDashVariables();
        TurnOffPlayerGotStuck();

        if (playerMovementFour.isCop28 && shootingCop28 != null) //TEST
        {
            shootingCop28.ResetArrowWhenPlayerIsdead();
        }
        else
        {
            Debug.Log("shootingCop28 script can't be found");
        }

    }
    public void GameOver()
    {
        inGameUI.SetActive(false);
        youDiedScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }
    public void Contunie()
    {
        playerKnockedDown = false; // for cop28
        TurnOffQTE();
        playerDeathAudio.Stop();
        youDiedScreen.SetActive(false);
        inGameUI.SetActive(true);
        Time.timeScale = 1f; //Unfreeze game
        if (player.activeInHierarchy == false)
        {
            player.SetActive(true);
        }
    }
    public void TurnOffPlayerGotStuck()
    {
        SharkAI[] sharksScripts = FindObjectsOfType<SharkAI>();
        foreach (SharkAI sharkAI in sharksScripts)
        {
            if(sharkAI.isActiveAndEnabled && sharkAI.playerGotStuck && sharkAI.cop28IsActive)
            {
                sharkAI.TurnOffPlayerGotStuck();
            }
        }
    }
    public void TurnOffQTE()
    {
        if (qTE1 != null)
        {
            qTE1.ResetEvent();
        }
        if (qTE2 != null)
        {
            qTE2.ResetEvent();
            //print("QTE2 RESET!");
        }
        if (qTE3 != null)
        {
            qTE3.ResetEvent();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (spawnPointIsReady == false)
        {
            if (collision.gameObject.tag == "Checkpoint")
            {
                spawnPointIsReady = true;
                spawnPointIsReady2 = false;
                spawnPointAudio.Play();
                //print("check point is ready");
                checkpoint1 = collision.gameObject.GetComponent<CheckpointHandler>();
                if(checkpoint2 != null)
                {
                    checkpoint2.ResetAnimation();
                }
            }
        }
        if (spawnPointIsReady2 == false)
        {
            if (collision.gameObject.tag == "Checkpoint2")
            {

                spawnPointIsReady = false;
                spawnPointIsReady2 = true;
                spawnPointAudio.Play();
                //print("check point 2 is ready");
                checkpoint2 = collision.gameObject.GetComponent<CheckpointHandler>();
                if (checkpoint1 != null)
                {
                    checkpoint1.ResetAnimation();
                }
            }
        }

    }
    /*
    public void TakingDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player Health = " + currentHealth);
        if (currentHealth <= 0)
        {
            GetComponent<PlayerMovement>().enabled = false;
            playerIsDead = true;
            cameraMovement.followPlayer = false;
            Invoke("Respawn", 1.5f);

            //player.gameObject.SetActive(false);
        }
    }
    public void Respawn()
    {
        Debug.Log("You are Dead, Back to spawn point");
        playerIsDead = false;
        currentHealth = maxHealth;
        player.transform.position = spawnPoint.position;
        GetComponent<PlayerMovement>().enabled = true;
        player.gameObject.SetActive(true);
    }
    */
}
