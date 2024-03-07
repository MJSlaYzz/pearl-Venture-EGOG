using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Cop28 Character Profile")]
public class Cop28CharacterProfile : ScriptableObject
{
    public string characterName;
    private Sprite characterPortrait;
    public Cop28EmotionType Emotion { set; get; }

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
        public Sprite casual;
        public Sprite curious_questioning;
        public Sprite surprise;
        public Sprite fear_pale;
        public Sprite nervous;
        public Sprite upset_regretful;
        public Sprite excited;
    }

    public EmotionPortraits emotionPortraits;
  

    public void setEmotionType(Cop28EmotionType newEmotion)
    {
        Emotion = newEmotion;

        switch (Emotion)
        {
            case Cop28EmotionType.Casual:
                characterPortrait = emotionPortraits.casual;
                break;

            case Cop28EmotionType.Curious_Questioning:
                characterPortrait = emotionPortraits.curious_questioning;
                break;

            case Cop28EmotionType.Surprise:
                characterPortrait = emotionPortraits.surprise;
                break;

            case Cop28EmotionType.Fear_Pale:
                characterPortrait = emotionPortraits.fear_pale;
                break;

            case Cop28EmotionType.Nervous:
                characterPortrait = emotionPortraits.nervous;
                break;
            case Cop28EmotionType.Upset_Regretful:
                characterPortrait = emotionPortraits.upset_regretful;
                break;
            case Cop28EmotionType.Excited:
                characterPortrait = emotionPortraits.excited;
                break;

        }
    }
}

public enum Cop28EmotionType
{
    Casual,
    Curious_Questioning,
    Surprise,
    Fear_Pale,
    Nervous,
    Upset_Regretful,
    Excited
}

