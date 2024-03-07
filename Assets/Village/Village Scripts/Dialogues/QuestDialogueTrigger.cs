using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDialogueTrigger : DialogueTrigger
{
    [Header("Quest Dialogue Info: ")]
    public QuestDialogue[] questDialogue;
    public bool hasAvailableQuest;
    
    public int questIndex { set; get; }

    public override void Interact()
    {
        if (hasAvailableQuest)
        {
            DialogueManager.instance.EnqueueDialogue(questDialogue[questIndex]);
            QuestManager.questManager.currentQuestDialogueTrigger = this; //A reference to the quest dialogue trigger we're interacting with.
        }
        else
        {
            base.Interact();
        }
    }
}

