using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignRopePoints : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private RopeController ropeController;

    // transform variables
    public float speed = 10;
    public List<Transform> targets = new List<Transform>();
    public Transform playerTransform;
    public Transform target;
    protected int currentTargetIndex = 0;

    bool reachedLocation = false;
    bool buttonIsPressed =false;
    void Start()
    {
        //transform
        target = targets[currentTargetIndex];

        ropeController.SetUpLine(points);
    }
    void Update()
    {
        //if (Input.GetKeyDown("space"))
        //{
        //    Debug.Log("space was pressed");
        //    buttonIsPressed = true;
        //}
        //RopeTransform(); // moves player to target
    }
    void RopeTransform()
    {
        if (!reachedLocation && buttonIsPressed)
        {
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, target.position, speed * Time.deltaTime);
            if (playerTransform.position == target.position)//check if you reached the target
            {
                reachedLocation = true;
                /* use it if you have more points.
                if (currentTargetIndex >= targets.Count)//check if you reached the last position in your targets list
                {
                    currentTargetIndex = 0;//go to first target in your target list
                }

                else
                {
                    currentTargetIndex++;// go to next target in your target list
                }
                target = targets[currentTargetIndex];// set the next target
                */
            }
        }

    }
}
