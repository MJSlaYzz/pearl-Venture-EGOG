using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCam : MonoBehaviour
{
    [SerializeField] Transform camPosition1;
    [SerializeField] Transform camPosition2;
    [SerializeField] Transform camPosition3;

    [HideInInspector] bool moveTo1 = false;
    [HideInInspector] bool moveTo2 = false;
    [HideInInspector] bool moveTo3 = false;

    [SerializeField] float cameraMovementSpeed = 10f;

    private void Start()
    {
        transform.position = camPosition1.position;
    }
    private void Update()
    {
        MoveToPosition1();
        MoveToPosition2();
        MoveToPosition3();
    }
    public void EnableBool(int boolNum)
    {
        if(boolNum == 1)
        {
            moveTo1 = true;
            moveTo2 = false;
            moveTo3 = false;
        }
        else if (boolNum == 2)
        {
            moveTo1 = false;
            moveTo2 = true;
            moveTo3 = false;
        }
        else if (boolNum == 3)
        {
            moveTo1 = false;
            moveTo2 = false;
            moveTo3 = true;
        }
    }
    void MoveToPosition1()
    {
        if (moveTo1 == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, camPosition1.position, cameraMovementSpeed * Time.deltaTime);
            if(transform.position == camPosition1.position)
            {
                moveTo1 = false;
            }
        }
    }
    void MoveToPosition2()
    {
        if (moveTo2 == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, camPosition2.position, cameraMovementSpeed * Time.deltaTime);
            if (transform.position == camPosition2.position)
            {
                moveTo2 = false;
            }
        }
    }
    void MoveToPosition3()
    {
        if (moveTo3 == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, camPosition3.position, cameraMovementSpeed * Time.deltaTime);
            if (transform.position == camPosition3.position)
            {
                moveTo3 = false;
            }
        }
    }
}
