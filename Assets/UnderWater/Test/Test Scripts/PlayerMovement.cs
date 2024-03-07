using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed =5f;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 localScale;
    public bool isLookingRight = true;
    public bool isLookingUp = true;

    [SerializeField] private Animator animator;

    private void Start()
    {
        //animator = GameObject.FindObjectOfType<Animator>();
    }
    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Movement();
        FlipBackUp(horizontalInput, verticalInput);
        localScale = transform.localScale;
        
    }
    void Movement()
    {
        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        float InputMagnitude = Mathf.Clamp01(movementDirection.magnitude); //store the value of the Direction and clamp it before normalize it.
        movementDirection.Normalize();
        transform.Translate(movementDirection * speed * InputMagnitude * Time.deltaTime, Space.World);
        //specifying that we are moving the character relative to the world by typing Space.World.
        HorizontalFlip(horizontalInput);
        VerticaleFlip(verticalInput);

        animator.SetFloat("Horizontal Speed", Mathf.Abs(horizontalInput));
        animator.SetFloat("Vertical Speed", Mathf.Abs(verticalInput));
    }
    void HorizontalFlip(float horizontalInput)
    {
        if (isLookingRight && horizontalInput < 0f || !isLookingRight && horizontalInput > 0f)
        {
            isLookingRight = !isLookingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    void VerticaleFlip(float verticalInput)
    {
        if(isLookingUp && verticalInput < 0f || !isLookingUp && verticalInput > 0f)
        {
            isLookingUp = !isLookingUp;
            localScale.y *= -1f;
            transform.localScale = localScale;
        }
    }
    void FlipBackUp(float horizontalInput, float verticalInput)
    {
        /*
        if ((!isLookingUp && verticalInput == 0f) && (!isLookingUp && horizontalInput > 0f) || (!isLookingUp && horizontalInput < 0f))
        {
            isLookingUp = !isLookingUp;
            localScale.y *= -1f;
            transform.localScale = localScale;
        }
        */
        if(!isLookingUp && verticalInput == 0f)
        {
            isLookingUp = !isLookingUp;
            localScale.y *= -1f;
            transform.localScale = localScale;
        }
        if(!isLookingUp && verticalInput < 0f && horizontalInput != 0f)
        {
            isLookingUp = !isLookingUp;
            localScale.y *= -1f;
            transform.localScale = localScale;
        }
    }
}
