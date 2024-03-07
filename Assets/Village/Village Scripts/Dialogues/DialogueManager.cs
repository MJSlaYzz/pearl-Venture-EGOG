using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text charName;
    public Text dialogueText;
    public Image charPortrait;
    

    public static DialogueManager instance;
    public GameObject dialogueBox;
    private DialogueBase currentDialogue;

    public bool inDialogue;
    public bool isCurrentlyTyping;
    private bool buffer;

    private string completeText;
    public float textDelay = 0.001f;

    public Queue<DialogueBase.Info> dialogueInfo = new Queue<DialogueBase.Info>();
    //Queue- similar to an array, allows you to store multiple elements of smth. Enqueue: Will add to a queue. Dequeue: Will remove from a queue.

    public void EnqueueDialogue(DialogueBase db) //Takes info from DialogueBase and adds it to the queue.
    {
        if (inDialogue || QuestManager.questManager.inQuestUI)
            return;

        inDialogue = true; 
        StartCoroutine(BufferTimer());
        buffer = true;
          
        dialogueBox.SetActive(true);
        dialogueInfo.Clear(); //clears previous texts in queue.
        currentDialogue = db;

        foreach(DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }

        DequeueDialogue(); //sets the first dialogue. 
            
    }


    public void DequeueDialogue()
    {
        
        
        if(isCurrentlyTyping)
        {
            if (buffer)
                return;

            CompleteText();
            StopAllCoroutines();
            isCurrentlyTyping = false;
            return;
        }
        if (dialogueInfo.Count == 0)//Checks when queue is empty --> ends dialogue.
        {
            EndofDialogue();
            return;
        }

        DialogueBase.Info info = dialogueInfo.Dequeue();
        completeText = info.charText; //Will be displayed when user clicks 'next line' mid typing.

        charName.text = info.character.characterName;
        dialogueText.text = info.charText;
        info.ChangeEmotion();
        charPortrait.sprite = info.character.CharacterPortrait;

        dialogueText.text = ""; //Clearing previous text;
        StartCoroutine(TypeText(info));
      
    }


    public void EndofDialogue()
    {
        dialogueBox.SetActive(false);
        inDialogue = false;
        CheckIfQuestDialogue();
    }

    private void CompleteText()
    {
        dialogueText.text = completeText;
    }

    IEnumerator TypeText(DialogueBase.Info info)
    {
        isCurrentlyTyping = true;

        foreach(char x in info.charText.ToCharArray())
        {
            yield return new WaitForSeconds(textDelay);
            dialogueText.text += x;
            
        }
        isCurrentlyTyping = false;
    }

    IEnumerator BufferTimer()
    {
        yield return new WaitForSeconds(0.01f);
        buffer = false;
    }

    public void Awake() // Singleton- ensures that only one instance of this class exists throughout the application's lifecycle
    {
        if (instance == null)
            instance = this;
    }
     
    private void CheckIfQuestDialogue()
    {
        if( currentDialogue is QuestDialogue)
        {
            QuestDialogue QD = currentDialogue as QuestDialogue;
            QuestManager.questManager.SetQuestUIInfo(QD.quest);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if (inDialogue)
                DequeueDialogue();
        }
    }
}
