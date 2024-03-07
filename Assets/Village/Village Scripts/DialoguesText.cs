using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguesText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextComponent;
    [SerializeField] private GameObject background;
    [SerializeField] private string[] lines;
    [SerializeField] private float textspeed;

    //[SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject tipsScreen1;

    [SerializeField] private VPlayerMovement playerMovement;

    [SerializeField] private AudioSource buttonClick;
    [SerializeField] private AudioClip[] audioClips;


    private int index;

    [SerializeField] private string[] dialoguesLines1;
    [SerializeField] private string[] dialoguesLines2;
    [SerializeField] private string[] dialoguesLines3;

    [SerializeField] private int dialoguesNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        DialougeBegin();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
        EndDialogue();
    }
    public void DialougeBegin()
    {
        playerMovement.enabled = false;
        background.SetActive(true);
        TextComponent.text = string.Empty;

        if (dialoguesNum == 0)
        {
            lines = dialoguesLines1;
        }
        else if (dialoguesNum == 1)
        {
            lines = dialoguesLines2;
        }
        else if (dialoguesNum == 2)
        {
            lines = dialoguesLines3;
        }
        startDialogue();
    }
    public void startDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());

    }
    public void restartDialogue()
    {
        TextComponent.text = string.Empty;
        startDialogue();
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            TextComponent.text += c;
            GetRandomItemArray(audioClips);
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
            TextComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }
    public void GetRandomItemArray(AudioClip[] arrayToRandomize)
    {
        int randomNum = Random.Range(0, arrayToRandomize.Length);
        AudioClip playRandom = arrayToRandomize[randomNum];
        buttonClick.clip = playRandom;
        buttonClick.Play();
    }

    public void EndDialogue()
    {
        print("index = " + index);
        print("lines.Length = " + lines.Length);

        if (index == lines.Length - 1)
        {
            print("text is done");

            if (TextComponent.text == lines[index] && Input.GetMouseButtonDown(0))
            {
                TextComponent.text = string.Empty;
                background.SetActive(false);
                PlayerCanMove();
            }
            //continueButton.SetActive(true);
        }
    }
    public void PlayerCanMove()
    {
        playerMovement.enabled = true;
    }
    public void SetDialougeValue(int dialougeValue)
    {
        dialoguesNum = dialougeValue;

    }
}
