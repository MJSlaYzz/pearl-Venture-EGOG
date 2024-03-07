using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName  = "New Dialogue", menuName = "Dialogues/Basic Dialogue")]

public class DialogueBase : ScriptableObject
{
    [System.Serializable]
    public class Info
    {
        public CharacterProfile character;
        [TextArea(5,10)]
        public string charText;

        public EmotionType characterEmotion;

        public void ChangeEmotion()
        {
            character.Emotion = characterEmotion;
        }
    }

    

    [Header("Dialogue Information: ")]
    public Info[] dialogueInfo;
}
