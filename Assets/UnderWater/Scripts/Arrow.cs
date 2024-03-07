using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region

    [HideInInspector] private Rigidbody2D rb;
    [HideInInspector] private bool hasHit;
    //[HideInInspector] private BoxCollider2D collider2D;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //collider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasHit == false)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            hasHit = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            //collider2D.enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;


        }
        else if (collision.gameObject.tag == "Shark")
        {
            //slow down the shark
            hasHit = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }
}
