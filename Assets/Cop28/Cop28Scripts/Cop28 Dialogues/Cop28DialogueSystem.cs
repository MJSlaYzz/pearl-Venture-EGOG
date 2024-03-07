using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cop28DialogueSystem : MonoBehaviour
{
    #region Variables
    [SerializeField] private TextMeshProUGUI TextComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float textspeed;

    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject tutorialScreen;
    [SerializeField] private GameObject tipsScreen1;
    private int index;
    //[SerializeField] private AudioSource buttonClick;
    //[SerializeField] private AudioClip[] audioClips;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
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
        if (index == lines.Length - 1)
        {
            continueButton.SetActive(true);
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
}
