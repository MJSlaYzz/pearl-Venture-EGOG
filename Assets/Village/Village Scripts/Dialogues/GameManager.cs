using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void OnItemCollectedCallBack(CollectableProfile collectableProfile); //So we can know what type of item was collected each time.
    public OnItemCollectedCallBack onItemCollectedCallBack;

    public Transform player;

    public static GameManager instance; //Singleton

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
