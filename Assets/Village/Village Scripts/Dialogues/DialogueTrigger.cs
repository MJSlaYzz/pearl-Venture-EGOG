using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interaction
{
    
    public DialogueBase dialogue;

    public override void Interact()
    {
        DialogueManager.instance.EnqueueDialogue(dialogue);

        Debug.Log("Interacting with NPC");
    }
}