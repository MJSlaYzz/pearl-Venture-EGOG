using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //camera clamp
    [SerializeField] private Transform targetToFollow;

    //[SerializeField] private float maxValueX = 14f;
    //[SerializeField] private float minValueX = -12.75f;
    //[SerializeField] private float maxValueY = 7.5f;
    //[SerializeField] private float minValueY = -4.3f;
    [SerializeField] private Vector2 minVlaue;
    [SerializeField] private Vector2 maxVlaue;
    [HideInInspector] public bool followPlayer = true;
    [SerializeField] Transform spawnpoint;
    //[SerializeField] private float cameraMovementToSpawnSpeed = 8f;

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        //MoveToSpawnPoint();
        GoToSpawnPoint();
    }
    void FollowPlayer()
    {
        if (followPlayer)
        {
            transform.position = new Vector3(
            Mathf.Clamp(targetToFollow.position.x, minVlaue.x, maxVlaue.x),
            Mathf.Clamp(targetToFollow.position.y, minVlaue.y, maxVlaue.y),
            transform.position.z);
        }

    }
    /*
    void MoveToSpawnPoint()
    {
        if (!followPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnpoint.position, cameraMovementToSpawnSpeed * Time.deltaTime);
            if (transform.position == spawnpoint.position)
            {
                followPlayer = true;
            }
        }
    }
    */
    void GoToSpawnPoint()
    {
        if (!followPlayer)
        {
            transform.position = spawnpoint.position;
            followPlayer = true;
        }
    }
}
