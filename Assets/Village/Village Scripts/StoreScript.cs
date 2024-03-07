using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreScript : MonoBehaviour
{
    public Quests quests;
    public bool Quest = false;
    public GameObject StoreDialog;

    public void Dialog()
    {
        if (!Quest)
        {
            if (!StoreDialog.activeInHierarchy)
            {
                StoreDialog.SetActive(true);
            }
           
        }
    }
    public void Accept()
    {
        quests.AddQuest();
        Quest = true;
    }
    public void Decline()
    {
        Quest = false;
    }
}
