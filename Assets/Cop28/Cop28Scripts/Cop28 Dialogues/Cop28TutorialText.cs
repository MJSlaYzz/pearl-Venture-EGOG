using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cop28TutorialText : MonoBehaviour
{
    // The Collider Triggers Are IN DialogueTriggerPoints.
    [SerializeField] private Text TextComponent;
    [TextArea(5, 10)]
    [SerializeField] private string[] lines;
    [SerializeField] private float textspeed = 0.01f;

    //[SerializeField] private GameObject tutorialScreen;

    [HideInInspector] private PlayerMovementFour playerMovement;
    [HideInInspector] private PlayerHealth playerHealth;
    [SerializeField] private OxygenSystem oxygenSystem;

    //[SerializeField] private AudioSource buttonClick;
    //[SerializeField] private AudioClip[] audioClips;

    [HideInInspector] private int lineStartNum;
    [HideInInspector] private int lineEndNum;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject endButton;
    [HideInInspector] private ShootingCop28 shootingCop28;
    [HideInInspector] private DialogueTriggerPoints dialogueTriggerPoints;

    // next and previous lines
    [SerializeField]  private int reachedLine;
    [SerializeField] private Button nextLineButton;
    [SerializeField] private Button previousLineButton;

    [SerializeField] private int index;

    // Start is called before the first frame update
    void Start()
    {
        //textStarted = false;
        playerMovement = FindObjectOfType<PlayerMovementFour>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        shootingCop28 = FindObjectOfType<ShootingCop28>();
        dialogueTriggerPoints = FindObjectOfType<DialogueTriggerPoints>();
        dialogueBox.SetActive(true);
        playerMovement.playerCanMove = false;
        TextComponent.text = string.Empty;
        SetStartingLine();
    }

    // Update is called once per frame
    void Update()
    {
        // ADD ARROWS TO MOVE BETWEEN LINES !!!!!!!!!!!!!
        if (playerHealth.playerKnockedDown || playerHealth.playerIsDead)
        {
            if (dialogueBox.activeInHierarchy)
            {
                dialogueBox.SetActive(false);
            }
        }
        if (index == lineEndNum)
        {
            if (TextComponent.text == lines[lineEndNum])
            {
                endButton.SetActive(true);//from this we will hide the dialogueBox.
            }
        }
        else
        {
            endButton.SetActive(false);
        }
        if (dialogueBox.activeInHierarchy && dialogueBox != null && !dialogueTriggerPoints.shootinUnlocked)
        {
            shootingCop28.shootingIsAllowed = false;
        }
        else if (dialogueTriggerPoints.shootinUnlocked && !dialogueBox.activeInHierarchy && dialogueBox != null)
        {
            shootingCop28.shootingIsAllowed = true;
        }
        if (TextComponent.text == lines[2]) // the line taking about player movement.
        {
            PlayerCanMove();
        }
        EnableAndDisableNextAndPreviousButtons();
        //if (oxygenSystem.lerpValue == 20f || (oxygenSystem.lerpValue > 19f && oxygenSystem.lerpValue < 20f))
        //{
        //    oxygenSystem.enabled = false;
        //}

    }
    void SetStartingLine() //get called at the start of the game.
    {
        //yield return new WaitForSeconds(0.01f); //small fix for lag issue (lag was fixed)
        lineStartNum = 0;
        lineEndNum = 6;
        //textStarted = true;
        StartCoroutine(TypeLine());
    }
    public void TakeOrderOfLines(int lineStartNum, int lineEndNum) //will get called from the Cop28DataManager script;
    {
        this.lineStartNum = lineStartNum;
        this.lineEndNum = lineEndNum;

        dialogueBox.SetActive(true);
        SetIndex();
    }
    public void SetIndex()
    {
        index = lineStartNum;
        CheckIfReachedEndLine();
    }
    public void CheckIfReachedEndLine() //get caleld from the nextline button.
    {
        if (TextComponent.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            TextComponent.text = lines[index];
        }
        
    }
    public void CloseDialogue() // clled from end button
    {
        dialogueBox.SetActive(false);
        PlayerCanMove();
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            TextComponent.text += c;
            //GetRandomItemArray(audioClips);
            yield return new WaitForSeconds(textspeed);
        }
    }
    //checks if the text finished and moves to the next one 
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            //TextComponent.text += "\n";
            index++;
            if(index > reachedLine)
            {
                reachedLine = index;
            }
            TextComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }
    public void PlayerCanMove()
    {
        playerMovement.playerCanMove = true;
        oxygenSystem.enabled = true;
    }
    public void NextLineButton()
    {
        if(index < reachedLine)
        {
            CheckIfReachedEndLine();
        }
    }
    public void PreviousLineButton()
    {
        if (index > 0)
        {
            index--;
            CheckIfReachedEndLine();
        }
    }
    void EnableAndDisableNextAndPreviousButtons()
    {
        if (index == reachedLine)
        {
            nextLineButton.interactable = false;
        }
        else
        {
            nextLineButton.interactable = true;
        }

        if(index == 0)
        {
            previousLineButton.interactable = false;
        }
        else
        {
            previousLineButton.interactable = true;
        }
    }
}
