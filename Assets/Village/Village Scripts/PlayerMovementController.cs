using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    
    private void Update()
    {
        if (QuestManager.questManager.inQuestUI || DialogueManager.instance.inDialogue)
        {
            VPlayerMovement.instance.enabled = false;
        }
        else
        {
            VPlayerMovement.instance.enabled = true;
        }
    }
}
