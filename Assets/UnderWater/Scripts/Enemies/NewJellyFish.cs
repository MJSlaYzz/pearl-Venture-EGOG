using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NewJellyFish : MonoBehaviour
{
    #region Variables
    [HideInInspector] private OxygenSystem oxygenSystem;
    [HideInInspector] private PlayerHealth playerHealth;
    [HideInInspector] private PlayerMovementFour playerMovementFour;
    [HideInInspector] bool alertAudioPlayed = false;
    [HideInInspector] AudioSource alertAudioSource;
    [SerializeField] AudioClip alertClip;
    [SerializeField] GameObject exclamationMark;
    [SerializeField] ParticleSystem electrifiedParticles;

    [HideInInspector] float normalMoveSpeed;
    [SerializeField] float debuffMoveSpeed = 3f;
    [HideInInspector] float normalDashSpeed;
    [SerializeField] float debuffDashSpeed = 300f;

    #endregion

    private void Start()
    {
        alertAudioSource = GetComponent<AudioSource>();
        oxygenSystem = FindObjectOfType<OxygenSystem>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerMovementFour = FindObjectOfType<PlayerMovementFour>();

        normalMoveSpeed = playerMovementFour.moveSpeed;
        normalDashSpeed = playerMovementFour.dashSpeed;
    }

   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(oxygenSystem != null) 
        {
            if (collision.tag == "Player")
            {
                // turning the Debuff on.
                oxygenSystem.jellyFishDebuff = true;

                // Changing the Particles size.
                var pMain = electrifiedParticles.main;
                pMain.startSize = 3f;

                // Changing Player's movement speed.
                playerMovementFour.moveSpeed = debuffMoveSpeed;
                playerMovementFour.dashSpeed = debuffDashSpeed;

                // Playing Alert Sound Effect once.
                if (!alertAudioPlayed && !playerHealth.playerIsDead)
                {
                    alertAudioSource.PlayOneShot(alertClip);
                    alertAudioPlayed = true;
                }

                // Showing the Alert Mark.
                if (exclamationMark != null && !exclamationMark.activeInHierarchy)
                {
                    exclamationMark.SetActive(true);
                }
                //Debug.Log("Buff ON");
            }
        }
        else
        {
            Debug.Log("OxygenSystem notfound");
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(oxygenSystem != null) 
        {
            if (collision.tag == "Player")
            {
                // turning the Debuff off.
                oxygenSystem.jellyFishDebuff = false;

                // Changing the Particles size back to normal.
                var pMain = electrifiedParticles.main;
                pMain.startSize = 1f;

                // Changing Player's movement speed back to normal.
                playerMovementFour.moveSpeed = normalMoveSpeed;
                playerMovementFour.dashSpeed = normalDashSpeed;

                // Now the Alert sound can play again.
                alertAudioPlayed = false;

                // Hiding the Alert Mark.
                if (exclamationMark != null && exclamationMark.activeInHierarchy)
                {
                    exclamationMark.SetActive(false);
                }
                //Debug.Log("Buff OFF");
            }
        }
        else
        {
            Debug.Log("OxygenSystem notfound");
        }
    }

}
