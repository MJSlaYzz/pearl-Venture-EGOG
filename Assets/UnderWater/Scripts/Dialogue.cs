using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI TextComponent;
    public string[] lines;
    public float textspeed;

    public AudioSource buttonClick;
    public AudioClip[] audioClips;

    public AudioSource voiceActingSource;
    public AudioClip[] voiceActingClips;

    //public AudioSource backgroundMusic;
    //public AudioSource backgroundWave;
    public bool lowVolume = false;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        TextComponent.text = string.Empty;
        startDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SkipDialogue();
        }
    }
    public void startDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());

    }
    public void SkipDialogue()
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
    public void restartDialogue()
    {
        TextComponent.text = string.Empty;
        StopAllCoroutines();
        startDialogue();
        PlayNextVoiceLine();
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
            index++;
            TextComponent.text = string.Empty;
            PlayNextVoiceLine();
            StartCoroutine(TypeLine());
        }
        /*
        else
            gameObject.SetActive(false);
        */
    }
    public void GetRandomItemArray(AudioClip[] arrayToRandomize)
    {
        int randomNum = Random.Range(0, arrayToRandomize.Length);
        AudioClip playRandom = arrayToRandomize[randomNum];
        buttonClick.clip = playRandom;
        buttonClick.Play();
    }
    void PlayNextVoiceLine()
    {
        voiceActingSource.clip = voiceActingClips[index];
        voiceActingSource.Play();
    }
    public void LowBackgroundMusic()
    {
        lowVolume = true;
        //backgroundMusic.volume = 0.1f;
        //backgroundWave.volume = 0.1f;
    }
    public void NormalBackgroundMusic()
    {
        
        lowVolume = false;
        //backgroundMusic.volume = 1f;
        //backgroundWave.volume = 0.5f;
    }
}