using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollectable : MonoBehaviour
{
    //JUST A TEST ATM. 
    public GameObject collectable;
    public CollectableProfile collectableProfile;
    public QuestCollect quest;

    private void Awake()
    {
        collectable.SetActive(false);
    }
    private void Update()
    {
        ShowCollectable();

        if (Vector2.Distance(collectable.transform.position, GameManager.instance.player.position) <= 1 && quest.isInitialized == true)
        {
           collectable.SetActive(false);
           quest.ItemCollected(collectableProfile);
           Collected();
        }
        
    }

    public void ShowCollectable()
    {
        if(quest.isInitialized == true)
        collectable.SetActive(true);
    }

    public void Collected()
    {
        if (GameManager.instance.onItemCollectedCallBack != null)
            GameManager.instance.onItemCollectedCallBack.Invoke(collectableProfile);

        gameObject.SetActive(false);
    }
}
