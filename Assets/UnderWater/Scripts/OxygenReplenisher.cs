using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenReplenisher : MonoBehaviour
{
    [SerializeField] OxygenSystem oxygenSystem;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip outOfWater;
    [HideInInspector] bool audioCanPlay = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("START OxygenReplenisher ");
            oxygenSystem.oxygenReplenisher = true;

            if(gameObject.name == "WaterSurface" && audioSource != null && audioCanPlay)
            {
                audioSource.PlayOneShot(outOfWater);
                audioCanPlay = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("END OxygenReplenisher ");
            oxygenSystem.oxygenReplenisher = false;
            audioCanPlay = true;
        }
    }
}
