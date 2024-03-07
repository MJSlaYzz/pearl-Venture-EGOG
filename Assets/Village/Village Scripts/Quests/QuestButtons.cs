using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestButtons : MonoBehaviour
{
    public void questAccepted() //hides UI and initializes the quest.
    {
        QuestManager.questManager.currentQuestDialogueTrigger.hasAvailableQuest = false; //If a quest is accepted, no other quest from this NPC should be available. 
        QuestManager.questManager.currentQuest.InitializeQuest();       
        QuestManager.questManager.questUI.SetActive(false);
        QuestManager.questManager.StartQuestBuffer();
         
    }

    public void questDeclined() //hides UI only.
    {
        
        //accesses questUI through singleton, then sets it as inactive. Could be done without singleton too.
        QuestManager.questManager.questUI.SetActive(false);
        QuestManager.questManager.StartQuestBuffer();
        
        
    }
}
