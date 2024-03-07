using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;


    private float horizontalInput;
    private float verticalInput;


    private Vector3 localScale;
    private bool isLookingRight = true;
    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        localScale = transform.localScale;
        Movement();
    }

    void Movement()
    {
        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude); //store the value of the Direction and clamp it before normalize it.
        movementDirection.Normalize();

        transform.Translate(movementDirection * speed * inputMagnitude * Time.deltaTime, Space.World);
        //specifying that we are moving the character relative to the world by typing Space.World.

        Rotation(movementDirection); // sending the variable movementDirection to Rotation method and calling it.
        Flip(horizontalInput);
    }
    public void Rotation(Vector2 movementDirection)
    {
        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            //we use Quarternion.LookRotation to creat a method looking at the desired direction.

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), rotationSpeed* Time.deltaTime);
        }
    }
    private void Flip(float horizontalInput)
    {
        if (isLookingRight && horizontalInput < 0f || !isLookingRight && horizontalInput > 0f)
        {
            isLookingRight = !isLookingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
