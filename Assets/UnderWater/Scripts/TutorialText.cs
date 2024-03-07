using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float textspeed;

    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject tutorialScreen;
    [SerializeField] private GameObject tipsScreen1;

    [SerializeField] private PlayerMovementFour playerMovement;
    [SerializeField] private OxygenSystem oxygenSystem;

    [SerializeField] private AudioSource buttonClick;
    [SerializeField] private AudioClip[] audioClips;


    private int index;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement.enabled = false;

        TextComponent.text = string.Empty;
        startDialogue();
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
        if( index == lines.Length - 1)
        {
            continueButton.SetActive(true);
        }
        if (oxygenSystem.lerpValue == 20f || (oxygenSystem.lerpValue > 19f && oxygenSystem.lerpValue < 20f))
        {
            oxygenSystem.enabled = false;
        }
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

    public void PlayerCanMove()
    {
        playerMovement.enabled = true;
        oxygenSystem.enabled = true;
    }
}
