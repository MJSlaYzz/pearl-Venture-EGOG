using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cop28DialogueManager : MonoBehaviour
{
    #region Variables

    [Header("Text Variables")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject endButton;
    [SerializeField] private Text charName;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image charPortrait;
    [SerializeField] private float textDelay = 0.001f;
    [HideInInspector] private string completeText;

    [Header("Test Colors Variables")]
    [SerializeField] private Color commentColor;
    [SerializeField] private Color chatColor;
    [SerializeField] private Color actionColor;

    [Header("Reference Variables")]
    [SerializeField] private Cop28DialogueBase currentDialogue;

    [HideInInspector] public bool inDialogue;
    [HideInInspector] private bool isCurrentlyTyping;
    [HideInInspector] private bool buffer;
    [HideInInspector] public bool canDequeue = true; //for Scene Manager

    [HideInInspector] public static Cop28DialogueManager instance;
    [HideInInspector] public Queue<Cop28DialogueBase.Info> dialogueInfo = new Queue<Cop28DialogueBase.Info>();
    [HideInInspector] private PlayerMovementFour playerMovement;
    //Queue- similar to an array, allows you to store multiple elements of smth. Enqueue: Will add to a queue. Dequeue: Will remove from a queue.

    #endregion

    private void Start()
    {
        EnqueueDialogue(currentDialogue);
        playerMovement = FindObjectOfType<PlayerMovementFour>();
        if (playerMovement != null)
        {
            playerMovement.playerCanMove = false;
        }
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    if (inDialogue)
        //        DequeueDialogue();
        //}
        if(Input.GetKeyDown(KeyCode.Space)) // for trailer
        {
            DequeueDialogue();
        }
        if(dialogueInfo.Count == 0 && nextButton != null && endButton != null)
        {
            nextButton.SetActive(false);
            endButton.SetActive(true);
        }
    }
    public void EnqueueDialogue(Cop28DialogueBase db) //Takes info from DialogueBase and adds it to the queue.
    {
        if (inDialogue)
            return;

        inDialogue = true; 
        StartCoroutine(BufferTimer());
        buffer = true;
          
        dialogueBox.SetActive(true);
        dialogueInfo.Clear(); //clears previous texts in queue.
        currentDialogue = db;

        foreach(Cop28DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }

        DequeueDialogue(); //sets the first dialogue.    
    }
    public void DequeueDialogue()
    {
        if (canDequeue)
        {
            if (isCurrentlyTyping)
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

            Cop28DialogueBase.Info info = dialogueInfo.Dequeue();
            completeText = info.charText; //Will be displayed when user clicks 'next line' mid typing.

            charName.text = info.character.characterName;
            dialogueText.text = info.charText;
            info.ChangeEmotion();
            charPortrait.sprite = info.character.CharacterPortrait;

            dialogueText.text = ""; //Clearing previous text;

            if (info.isComment)
            {
                //print("comment");
                if (info.isAction)
                {
                    dialogueText.color = actionColor;
                }
                else
                {
                    dialogueText.color = commentColor;
                }
                dialogueText.alignment = TextAnchor.MiddleCenter;
                charName.enabled = false;
                charPortrait.enabled = false;
            }
            else
            {
                //print("no comment");
                dialogueText.alignment = TextAnchor.MiddleLeft;
                dialogueText.color = chatColor;
                charName.enabled = true;
                charPortrait.enabled = true;
            }
            StartCoroutine(TypeText(info));
            //CheckToHideDialogueBox(info);
        }
    }
    //#region Tutorial Methods
    //void CheckToHideDialogueBox(Cop28DialogueBase.Info info)
    //{
    //    if (info.StopAfterThis)
    //    {
    //        //print("StopAfterThis");
    //        playerMovement.playerCanMove = true;
    //        inDialogue = false;
    //        dialogueBox.SetActive(false);
    //    }
    //}
    //public void ShowDialogueBox()
    //{
    //    playerMovement.playerCanMove = false;
    //    inDialogue = true;
    //    dialogueBox.SetActive(true);
    //    DequeueDialogue();
    //}
    //#endregion

    public void EndofDialogue()
    {
        //print("END");
        if(playerMovement != null)
        {
            playerMovement.playerCanMove = true;
        }
        dialogueBox.SetActive(false);
        inDialogue = false;
    }

    private void CompleteText()
    {
        dialogueText.text = completeText;
    }

    IEnumerator TypeText(Cop28DialogueBase.Info info)
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

}
