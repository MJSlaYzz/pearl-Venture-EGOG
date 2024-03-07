using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Profile")]
public class CharacterProfile : ScriptableObject
{
    public string characterName;
    private Sprite characterPortrait;
    public EmotionType Emotion { set; get; }

    public Sprite CharacterPortrait
    {
        get
        {
            setEmotionType(Emotion);
            return characterPortrait;
        }
    }

    [System.Serializable]
    public class EmotionPortraits
    {
        public Sprite standard;
        public Sprite happy;
        public Sprite sad;
        public Sprite shocked;
        public Sprite angry;
    }

    public EmotionPortraits emotionPortraits;
  

    public void setEmotionType(EmotionType newEmotion)
    {
        Emotion = newEmotion;

        switch (Emotion)
        {
            case EmotionType.Standard:
                characterPortrait = emotionPortraits.standard;
                break;

            case EmotionType.Happy:
                characterPortrait = emotionPortraits.happy;
                break;

            case EmotionType.Sad:
                characterPortrait = emotionPortraits.sad;
                break;

            case EmotionType.Shocked:
                characterPortrait = emotionPortraits.shocked;
                break;

            case EmotionType.Angry:
                characterPortrait = emotionPortraits.angry;
                break;

        }
    }
}

public enum EmotionType
{
    Standard,
    Happy,
    Sad,
    Shocked,
    Angry
}

