using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestBase : ScriptableObject
{
    public string questName;
    [TextArea(5, 15)]
    public string questDescription;

    public bool isCompleted { set; get; }//Made an array to track complex quests with multiple objectives.
    public int[] requiredAmount { set; get; }
    public int[] currentAmount { set; get; }

    public CharacterProfile endPointNPC; //*To be replaced with a character profile script.
 
    

    public virtual void InitializeQuest() //Made virtual in case it needs to be overriden in specific derived classes. 
    {
        currentAmount = new int[requiredAmount.Length];
    }

    public void CompletionCheck() //A quest is marked complete when current >= required for all objectives. Otherwise, exit the method.
    {
        for (int x = 0; x < requiredAmount.Length; x++)
        {

            if (currentAmount[x] < requiredAmount[x])
            {
                return;
            }

        }

            Debug.Log("Quest is complete.");
                  
    }


  //Atm, this base only has 1 quest type that derives from it (QuestCollect). More types should be added.
}
