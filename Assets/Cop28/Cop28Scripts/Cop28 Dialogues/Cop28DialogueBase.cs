using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName  = "New Dialogue", menuName = "Dialogues/Basic Dialogue")]

public class Cop28DialogueBase : ScriptableObject
{
    [System.Serializable]
    public class Info
    {
        public Cop28CharacterProfile character;
        [TextArea(5,10)]
        public string charText;

        public Cop28EmotionType characterEmotion;
        //public bool StopAfterThis = false;
        public bool isComment = false;
        public bool isAction = false;
        public void ChangeEmotion()
        {
            character.Emotion = characterEmotion;
        }
    }

    

    [Header("Dialogue Information: ")]
    public Info[] dialogueInfo;
}
