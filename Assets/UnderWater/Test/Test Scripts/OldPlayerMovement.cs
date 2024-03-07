using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerMovement : MonoBehaviour
{
    private Vector2 movement;
    private Vector3 localScale;
    private bool isLookingRight = true;
    [SerializeField]private float speed = 15f;
    [SerializeField] Rigidbody2D rb;


    //Damage Impact Vriavles
    //[SerializeField] public Transform enemyTransform;
    //[SerializeField] public int damageforce = 5;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    private void Update() // update used to register input
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement.Normalize(); // It makes the velocity of the object on the chord equal to the velocity of the object on the straight line
        localScale = transform.localScale;
    }
    private void FixedUpdate() // by defult it;s called 50 times per secound
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        Flip();
        //rb.velocity = new Vector2(horizontal,vertical * speed * Time.fixedDeltaTime);
        //rb.velocity = movement * speed * Time.fixedDeltaTime;
    }
    private void Flip()
    {
        if(isLookingRight && movement.x < 0f || !isLookingRight && movement.x > 0f)
        {
            isLookingRight = !isLookingRight;
            localScale.x *= -1f; 
            transform.localScale = localScale;
        }
    }
    
   /* public void DamageImpact()
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (enemyTransform.position.x > transform.position.x)
                {
                    // enemy is to my right therefore I should be damaged and move left
                    rb.velocity = new Vector2(-damageforce, rb.velocity.y);
                }
                if (enemyTransform.position.x < transform.position.x)
                {
                    rb.velocity = new Vector2(damageforce, rb.velocity.y);
                    //enemy is to my left therefore I should be damaged and move right 
                }
                if (enemyTransform.position.y < transform.position.y)
                {
                    rb.velocity = new Vector2(rb.velocity.x, damageforce);
                    //enemy is under me therefore I should be damaged and move up 
                }
                if (enemyTransform.position.y > transform.position.y)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -damageforce);
                    //enemy is above me therefore I should be damaged and move down 
                }
            }
        }

    }
   */

}
