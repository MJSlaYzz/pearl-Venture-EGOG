using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using KeyCode = UnityEngine.InputSystem.Key;

public enum QTETimeType1
{
    Normal,
    Slow,
    Paused
}

public enum QTEPressType1
{
    Single,
    Simultaneously
}

[System.Serializable]
public class QuickTimeEventKey
{
    public KeyCode keyboardKey;
}

[System.Serializable]
public class QTEUI1
{
    public GameObject eventUI;
    public Text eventText;
    public Text eventTimerText;
    public Image eventTimerImage;
}
public class QuickTimeEvent : MonoBehaviour
{
    #region Variables
    [Header("Event settings")]
    public List<QuickTimeEventKey> keys = new List<QuickTimeEventKey>();
    public QTETimeType1 timeType;
    public float time = 3f;
    public bool failOnWrongKey;
    public QTEPressType1 pressType;
    [Header("UI")]
    public QTEUI1 keyboardUI;
    [Header("Event actions")]
    public UnityEvent onStart;
    public UnityEvent onEnd;
    public UnityEvent onSuccess;
    public UnityEvent onFail;

    public List<KeyCode> orignalKeysList = new List<KeyCode>();
    public List<KeyCode> randomKeysList = new List<KeyCode>();
    #endregion


    private void Start()
    {
        orignalKeysList = new List<KeyCode>() { KeyCode.E, KeyCode.Q, KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D};
        foreach (KeyCode key in orignalKeysList)
        {
            randomKeysList.Add(key);
        }
    }
    private void Update()
    {
        //print("randomKeysList.Count = " + randomKeysList.Count);
    }
    public void resetKeys()
    {
        randomKeysList.Clear();
        foreach (KeyCode key in orignalKeysList)
        {
            randomKeysList.Add(key);
        }

    }
    public void randomizeKeys()
    {
        if(randomKeysList.Count > 0 && randomKeysList != null)
        {
            int index = Random.Range(0, randomKeysList.Count);
            KeyCode randomKey = randomKeysList[index];
            keys[0].keyboardKey = randomKey;
            randomKeysList.Remove(randomKeysList[index]);
            //Debug.Log("Key1 " + keys[0].keyboardKey + " changed to: " + randomKey);
            index = Random.Range(0, randomKeysList.Count);
            KeyCode randomKey1 = randomKeysList[index];
            keys[1].keyboardKey = randomKey1;
            randomKeysList.Remove(randomKeysList[index]);
            //Debug.Log("Key2 " + keys[1].keyboardKey + " changed to: " + randomKey);
        }
        else
        {
            resetKeys();
            randomizeKeys();
        }
    }
}
