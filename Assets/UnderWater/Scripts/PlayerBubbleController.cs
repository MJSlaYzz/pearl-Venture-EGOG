using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBubbleController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ParticleSystem bubblesParticle;
    [SerializeField] private PlayerMovementFour playerMovementFour;

    [SerializeField] private Transform bubbleIdelRightPos;
    [SerializeField] private Transform bubbleIdelLeftPos;
    [SerializeField] private Transform bubbleUpPos;
    [SerializeField] private Transform bubbleDownPos;
    [SerializeField] private Transform bubbleRightPos;
    [SerializeField] private Transform bubbleLeftPos;
    [SerializeField] private Transform bubble315Pos;
    [SerializeField] private Transform bubble135Pos;
    [SerializeField] private Transform bubble225Pos;
    [SerializeField] private Transform bubble45Pos;
    [SerializeField] private Transform bubbleFightRightPos;
    [SerializeField] private Transform bubbleFightLeftPos;


    private float horizontalMovement;
    private float verticalMovement;

    private float timer;
    private bool shouldLerp = true;
    private float smoothTime = 5f; //timeBetweenBreaths
    private float lerpValue;
    private float numToLerpFrom = 5f;


    // Update is called once per frame
    void Update()
    {
        BubblesPositionController();
        LerpDown();
        LerpUp();
    }
    void LerpDown()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (shouldLerp && timer >= 0)
        {
            timer += 1 * Time.deltaTime;
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            lerpValue = Mathf.Lerp(numToLerpFrom, 0, inverseT);

        }
        if (lerpValue <= 0)
        {
            bubblesParticle.Play();
            shouldLerp = false;
        }
    }
    void LerpUp()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (!shouldLerp && timer >= 0)
        {
            timer -= 1 * Time.deltaTime * 50f;
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            lerpValue = Mathf.Lerp(numToLerpFrom, 0, inverseT);
        }
        if (lerpValue >= numToLerpFrom)
        {
            shouldLerp = true;
        }
    }
    void BubblesPositionController()
    {
        horizontalMovement = playerMovementFour.movement.x;
        verticalMovement = playerMovementFour.movement.y;


        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Animation left"))
        {
            //print("idel Animation left");
            transform.position = bubbleIdelLeftPos.position;
            transform.rotation = bubbleIdelLeftPos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Animation right"))
        {
            //print("idel Animation right");
            transform.position = bubbleIdelRightPos.position;
            transform.rotation = bubbleIdelRightPos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Movement") && horizontalMovement == 0 && verticalMovement == 1)
        {
            //print("swimming up Animation");
            transform.position = bubbleUpPos.position;
            transform.rotation = bubbleUpPos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Movement") && horizontalMovement == 0 && verticalMovement == -1)
        {
            //print("swimming down Animation 1");
            transform.position = bubbleDownPos.position;
            transform.rotation = bubbleDownPos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Movement") && horizontalMovement == 1 && verticalMovement == 0)
        {
            //print("swimming right Animation 3");
            transform.position = bubbleRightPos.position;
            transform.rotation = bubbleRightPos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Movement") && horizontalMovement == -1 && verticalMovement == 0)
        {
            //print("swimming left Animation 2");
            transform.position = bubbleLeftPos.position;
            transform.rotation = bubbleLeftPos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Movement") && horizontalMovement == 1 && verticalMovement == 1)
        {
            //print("player swimming 315");
            transform.position = bubble315Pos.position;
            transform.rotation = bubble315Pos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Movement") && horizontalMovement == -1 && verticalMovement == -1)
        {
            //print("swimming 135 Animation 1");
            transform.position = bubble135Pos.position;
            transform.rotation = bubble135Pos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Movement") && horizontalMovement == -1 && verticalMovement == 1)
        {
            //print("swimming 225 Animation 1");
            transform.position = bubble225Pos.position;
            transform.rotation = bubble225Pos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Movement") && horizontalMovement == 1 && verticalMovement == -1)
        {
            //print("swimming 45 Animation 1");
            transform.position = bubble45Pos.position;
            transform.rotation = bubble45Pos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("player fight left"))
        {
            //print("fight Animation left");
            transform.position = bubbleFightLeftPos.position;
            transform.rotation = bubbleFightLeftPos.rotation;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("player fight right"))
        {
            //print("fight Animation right");
            transform.position = bubbleFightRightPos.position;
            transform.rotation = bubbleFightRightPos.rotation;
        }
    }
        
}
