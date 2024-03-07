using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collect Quest", menuName = "Collect Quest")]
public class QuestCollect : QuestBase //Derives from QuestBase to specify the type of quest.
{
    [System.Serializable]
    public class Objectives //each objective has a required number of collectable.
    {
        public CollectableProfile collectable;
        public int requiredAmount;
    }

    public Objectives[] objectives;
    public bool isInitialized { set; get; }


    public override void InitializeQuest() //Overrides the one is QuestBase. 
    {
        GameManager.instance.onItemCollectedCallBack += ItemCollected; //We added ItemCollected to the delegate, so whenever

        requiredAmount = new int[objectives.Length];
        
        for (int x = 0; x < objectives.Length; x++) //setting up the required amount array. 
        {
            requiredAmount[x] = objectives[x].requiredAmount;
        }

        base.InitializeQuest();
        isInitialized = true;
    }

    public void ItemCollected(CollectableProfile collectable)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (collectable == objectives[i].collectable)
            {
                currentAmount[i]++;
            }
        }
        CompletionCheck();
    }


    /*
     This script is incomplete- What's required:
         A script for collectables as the required collectable field.
         Once we create a new quest, the collectable will be dragged into the required collectable field.

     To be modified: 
        The isItemCollected method to mark the quest as complete once the item is collected. 
        ^ Procedure for collecting an item could be in Collectable script.
    */
}
