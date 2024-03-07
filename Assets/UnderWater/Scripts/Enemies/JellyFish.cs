using System;
using System.Collections;
using Pathfinding.Util;
using UnityEngine;

public class JellyFish : MonoBehaviour
{
    #region Variables
    [SerializeField] CircleCollider2D jellyfishCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    
    bool coroutineRan = true;
    
    bool charged;
    [Header("Jellyfish Attributes")]
    [SerializeField] float chargeTime = 2f;  // time in seconds for the charge to build up
    [SerializeField] float waitTime = 2f;  // time in seconds for waiting for next iteration
    [SerializeField] float DebuffTime = 2f;
    [SerializeField] float DebuffMaxTime = 2f;

    bool playerInRange;
    bool playerLeavingRange;

    #endregion
    
    void Start()
    {
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        if (coroutineRan)
        {
            StartCoroutine(chargingForAOE());
            coroutineRan = false;
        }

        if (playerLeavingRange & DebuffTime > 0)
        {
            DebuffTime -= 1 * Time.deltaTime;
        }

        if (DebuffTime < 0.1 & !playerInRange)
        {
            GameEvents.current.jellyFishEventOff();
        }
        
        //Debug.Log(DebuffTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered Jelly's Range");
            playerInRange = true;
            playerLeavingRange = false;
        }
    }
    
    IEnumerator chargingForAOE()
    {
        //Debug.Log("Running Coroutine");
        
        yield return new WaitForSeconds(chargeTime);
        spriteRenderer.enabled = true;
        charged = true;
        
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.enabled = false;
        
        coroutineRan = true;
        //Debug.Log("Ending Coroutine");
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerInRange & charged)
            {
                GameEvents.current.jellyFishEventOn();
            
                if (DebuffTime <= DebuffMaxTime)
                {
                    DebuffTime += 1 * Time.deltaTime;
                }
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Exited Jelly's Range");
            playerInRange = false;
            playerLeavingRange = true;
            charged = false;
        }
    }
    
     void OnDrawGizmosSelected()
    {
        // Draws the wire sphere aligned with the collider, as long as the scale of the object has not been increased
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(jellyfishCollider.transform.position, jellyfishCollider.radius);
    }
}
