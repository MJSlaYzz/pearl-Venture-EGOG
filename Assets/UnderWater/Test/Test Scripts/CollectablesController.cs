using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectablesController : MonoBehaviour
{

    //[SerializeField] private int pealsCount = 0;
    //[SerializeField] public int PearlsTotal;
    [SerializeField] private float timeBetweenPhases = 2f;

    [SerializeField] private CollectablesCounter collectablesCounter;

    [SerializeField] private ParticleSystem plusOneParticle;

    private SpriteRenderer currenSprite;
    [SerializeField] private Sprite closedOyster;
    [SerializeField] private Sprite emptyOyster;
    [SerializeField] private Sprite pearlOyster;
    //public Image pearlIcon;

    private bool canBeRepeated = true;
    //private bool canBeCollected = false;
    private bool isCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        currenSprite = this.GetComponent<SpriteRenderer>(); //getting it from the script insted of the inspector
        currenSprite.sprite = closedOyster;
    }

    // Update is called once per frame
    void Update()
    {
        //Invoke("PearlPhases", 2f);
        if (canBeRepeated && Time.timeScale !=0) //fix the issue of oyster closing and opening in pause menu.
        {
            StartCoroutine(PearlPhases());
        }

    }

    public IEnumerator PearlPhases()
    {
        if (isCollected == false)
        {
            canBeRepeated = false;
            if(Time.timeScale != 0)
            {
                            // yield return is what tells Unity to pause the script and continue on the next frame
            yield return new WaitForSecondsRealtime(timeBetweenPhases);
            currenSprite.sprite = pearlOyster; //open the Oyster
            //canBeCollected = true; // now the player can collect the pearl
            yield return new WaitForSecondsRealtime(timeBetweenPhases);
            if (!isCollected) //make sure it wasn't collected while the Method was running
            {
                currenSprite.sprite = closedOyster; //close the Oyster
                //canBeCollected = false; // now the player can not collect the pearl
            }
            canBeRepeated = true;
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if(canBeCollected){...}
        if (isCollected == false && currenSprite.sprite == pearlOyster) //makes sure it wasn't already collected
        {
            if (collision.gameObject.tag == "Player")
            {
                plusOneParticle.Play();
                currenSprite.sprite = emptyOyster;
                isCollected = true; //can't be collected anymore
                //pearls counter
                //pealsCount++;
                collectablesCounter.AddAPearl();
                Debug.Log("Add one more pearl");
                //PearlsTotal = pealsCount;
                //Debug.Log("Total Pearls are: " + PearlsTotal);              
            }
        }
    }
}
