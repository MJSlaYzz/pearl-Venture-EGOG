using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPlayerMovement : MonoBehaviour

{ 

    #region Variables
    // Movement Variables
    [SerializeField] public float moveSpeed = 5f;
    [HideInInspector] private Animator animator;
    public bool isLookingRight = true;
    public Vector2 movement;
    
    public static VPlayerMovement instance { set; get; } //To create a singleton

    #endregion
    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        
    }
   
    private void Update()
    {     
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        /*
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("isLookingRight", isLookingRight);
        */
    }
    private void FixedUpdate()
    {
        Movement();
        DirectionCheck();

    }
    void Movement()
    {
        
        Vector2 movementDirection = new Vector2(movement.x, movement.y);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude); //store the value of the Direction and clamp it before normalize it.
        movementDirection.Normalize();

        transform.Translate(movementDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);
        //specifying that we are moving the character relative to the world by typing Space.World.
    }
    void DirectionCheck()
    {
        if (movement.x > 0.01)
        {
            isLookingRight = true;
            //Debug.Log(" player is Looking Right");
        }
        else if (movement.x < -0.01)
        {
            isLookingRight = false;
            //Debug.Log(" player is Looking Left");
        }
    }

}
