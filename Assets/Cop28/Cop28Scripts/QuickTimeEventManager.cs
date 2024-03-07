using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuickTimeEventManager : MonoBehaviour
{
    [Header("Configuration")]
    public float slowMotionTimeScale = 0.1f;
    public int phasesRepeatTimes = 2; // how many times will the it repeat.
    public Gradient imageColorGradient;
    public QTEUI1 qTEUI1;
    private int phasesCount = 0;

    [Header("UnLock")]
    public Image lockImage;
    public Sprite openedLockImage;
    public Sprite closedLockImage;
    public Animation lockAnimation;
    public AudioSource audioSource;
    public AudioClip unlockAudioClip;
    public AudioClip lockStuckAudioClip;

    [HideInInspector]
    private bool isEventStarted;
    private QuickTimeEvent eventData;
    private bool isAllButtonsPressed;
    private bool isFail;
    private bool isEnded;
    private bool isPaused;
    private bool wrongKeyPressed;
    private float currentTime;
    private float smoothTimeUpdate;
    private float rememberTimeScale;
    private List<QuickTimeEventKey> keys = new List<QuickTimeEventKey>();
    private PlayerMovementFour playerMovement;
    public bool eventSuceeded = false;
    public bool eventStarted = false;



    // Update is called once per frame
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementFour>();
    }
    void Update()
    {
        qTEUI1.eventTimerImage.color = imageColorGradient.Evaluate(qTEUI1.eventTimerImage.fillAmount);
        if (!isEventStarted || eventData == null || isPaused) return;
        updateTimer();
        if (keys.Count == 0 || isFail)
        {
            //doFinally();
            if (phasesCount == phasesRepeatTimes)
            {
                doFinally();
            }
            else
            {
                Phases();
            }
        }
        else
        {
            //Debug.Log("Checking keys");
            for (int i = 0; i < eventData.keys.Count; i++)
            {
                checkKeyboardInput(eventData.keys[i]);
            }
            for (int i = 0; i < keys.Count; i++) //prevent having more then 2 keys and getting stuck.
            {
                if(i > 1)
                {
                    keys.RemoveAt(i);
                    print("Extra Key Removed from keys list!!");
                }
            }
        }
    }
    public void startEvent(QuickTimeEvent eventScriptable)
    {
        //Stop player movement.
        StopAllCoroutines();
        if(playerMovement != null)
        {
            //playerMovement.enabled = false;
            playerMovement.playerCanMove = false;

        }
        else
        {
            print("Player movement script can't be found!");
        }
        eventData = eventScriptable;
        eventData.randomizeKeys();
        phasesCount = 0;
        phasesCount++;
        if (Keyboard.current == null)
        {
            Debug.Log("No keyboard connected. Gamepad input in QTE events is not supported!");
            return;
        }
        keys = new List<QuickTimeEventKey>(eventData.keys);
        if (eventData.onStart != null)
        {
            eventData.onStart.Invoke();
        }
        isAllButtonsPressed = false;
        isEnded = false;
        isFail = false;
        isPaused = false;
        eventSuceeded = false;
        eventStarted = true;
        rememberTimeScale = Time.timeScale;
        switch (eventScriptable.timeType)
        {
            case QTETimeType1.Slow:
                Time.timeScale = slowMotionTimeScale;
                break;
            case QTETimeType1.Paused:
                Time.timeScale = 0;
                break;
        }
        currentTime = eventData.time;
        smoothTimeUpdate = currentTime;
        setupGUI();
        StartCoroutine(countDown());
    }

    private void Phases()
    {
        eventData.randomizeKeys();
        phasesCount++;

        keys = new List<QuickTimeEventKey>(eventData.keys);
        isAllButtonsPressed = false;
        setupGUI();
    }
    private IEnumerator countDown()
    {
        isEventStarted = true;
        while (currentTime > 0 && isEventStarted && !isEnded)
        {
            if (eventData.keyboardUI.eventTimerText != null)
            {
                eventData.keyboardUI.eventTimerText.text = currentTime.ToString();
            }
            currentTime--;
            yield return new WaitWhile(() => isPaused);
            yield return new WaitForSecondsRealtime(1f);
        }
        if (!isAllButtonsPressed && !isEnded)
        {
            isFail = true;
            doFinally();
        }
    }

    public void ResetEvent()
    {
        if (isEventStarted)
        {
            isEventStarted = false;
            var ui = getUI();
            if (ui.eventUI != null)
            {
                ui.eventUI.SetActive(false);
            }
            if (eventData.onEnd != null)
            {
                eventData.onEnd.Invoke();
            }
            if (eventData.onFail != null && isFail)
            {
                eventData.onFail.Invoke();
            }
            if (eventData.onSuccess != null && isAllButtonsPressed)
            {
                eventData.onSuccess.Invoke();
            }
            eventData = null;
        }
        keys.Clear();
        isAllButtonsPressed = true;
        isEnded = true;
        isEventStarted = false;
        Time.timeScale = rememberTimeScale;

    }
    protected void doFinally()
    {
        //print("do finally");
        //Allow player movement.
        //if (playerMovement != null)
        //{
        //    playerMovement.enabled = true;
        //}
        StartCoroutine(ReleasePlayer());
        if (keys.Count == 0)
        {
            isAllButtonsPressed = true;
        }
        eventStarted = false;
        isEnded = true;
        isEventStarted = false;
        Time.timeScale = rememberTimeScale;
        var ui = getUI();
        if (ui.eventUI != null)
        {
            ui.eventUI.SetActive(false);
        }
        if (eventData.onEnd != null)
        {
            eventData.onEnd.Invoke();
        }
        if (eventData.onFail != null && isFail)
        {
            eventData.onFail.Invoke();
        }
        if (eventData.onSuccess != null && isAllButtonsPressed)
        {
            eventData.onSuccess.Invoke();
        }
        eventData = null;
    }
    private IEnumerator ReleasePlayer()
    {
        if (!isFail)
        {
            eventSuceeded = true;
            /*
            lockImage.sprite = closedLockImage;
            lockImage.enabled = true;
            yield return new WaitForSecondsRealtime(1f);
            lockImage.sprite = openedLockImage;
            audioSource.clip = unlockAudioClip;
            audioSource.Play();
            yield return new WaitForSecondsRealtime(0.5f); 
            lockImage.enabled = false;
            */
            yield return new WaitForSecondsRealtime(0.1f); //insted of the lines above to make the player cut atimation smooth in crab trap script.
            if (playerMovement != null)
            {
                //playerMovement.enabled = true;
                playerMovement.playerCanMove = true;
            }
            else
            {
                print("Player movement script can't be found!");
            }
        }
        else
        {
            //lockImage.sprite = closedLockImage; //for turtletrap 
            //lockImage.enabled = true;
            lockAnimation.Play();
            audioSource.clip = lockStuckAudioClip;
            audioSource.Play();
            yield return new WaitForSecondsRealtime(0.5f); // made it 0.5 insted 1.5 for turtletrap 
            lockImage.enabled = false;
            if (playerMovement != null)
            {
                //playerMovement.enabled = true;
                playerMovement.playerCanMove = true;
            }
            else
            {
                print("Player movement script can't be found!");
            }
        }

    }
    //OnGUI is called for rendering and handling GUI events.
    protected void OnGUI()
    {
        if (eventData == null || isEnded) return;
        if (Event.current.isKey && Event.current.type == EventType.KeyDown && eventData.failOnWrongKey && !Event.current.keyCode.ToString().Equals("None"))
        {
            wrongKeyPressed = true;
            eventData.keys.ForEach(key => wrongKeyPressed = wrongKeyPressed && !key.keyboardKey.ToString().Equals(Event.current.keyCode.ToString()));

            isFail = wrongKeyPressed;
        }
    }

    protected void updateTimer()
    {
        smoothTimeUpdate -= Time.unscaledDeltaTime;
        var ui = getUI();
        if (ui.eventTimerImage != null)
        {
            ui.eventTimerImage.fillAmount = smoothTimeUpdate / eventData.time;
        }
    }

    public void pause()
    {
        isPaused = true;
    }

    public void play()
    {
        isPaused = false;
    }
    public void checkKeyboardInput(QuickTimeEventKey key)
    {
        var inputType = Keyboard.current;
        if (inputType != null)
        {

            if (inputType[key.keyboardKey].wasPressedThisFrame)
            {
                keys.Remove(key);
                //Debug.Log("Key " + key +" got removed");
            }
            if (inputType[key.keyboardKey].wasReleasedThisFrame && eventData.pressType == QTEPressType1.Simultaneously)
            {
                keys.Add(key);
                //Debug.Log("Key " + key + " got added");
            }
        }
    }
    protected void setupGUI()
    {
        var ui = getUI();

        if (ui.eventTimerImage != null)
        {
            ui.eventTimerImage.fillAmount = 1;
        }
        if (ui.eventText != null)
        {
            ui.eventText.text = "";
            eventData.keys.ForEach(key => ui.eventText.text += key.keyboardKey + "+");
            eventData.keyboardUI.eventText.text = ui.eventText.text.Remove(ui.eventText.text.Length - 1);
        }
        if (ui.eventUI != null)
        {
            ui.eventUI.SetActive(true);
        }
    }
    protected QTEUI1 getUI()
    {
        var ui = eventData.keyboardUI;
        return ui;
    }
}

