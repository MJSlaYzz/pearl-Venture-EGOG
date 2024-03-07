using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour

{

    public float interactRange = 5;
    //public bool Range;
   // public KeyCode interactionBtn = KeyCode.E;
   // public UnityEvent Action;
   

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(gameObject.transform.position, GameManager.instance.player.position) < interactRange)
        {
            if(Input.GetKeyDown(KeyCode.T))
            Interact();
        }

        /*if (Range)
        {
            if (Input.GetKeyDown(interactionBtn))
            {
                Debug.Log("Action Done");
                Action.Invoke();
            }
        }*/

    }

    public virtual void Interact() //Will be overriden in other scripts.
    {

    }

    private void OnDrawGizmosSelected()
    {        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Range = true;
            Debug.Log("Player is In Range");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Range = false;
            Debug.Log("Player is not In Range");
        }
    }*/

}
