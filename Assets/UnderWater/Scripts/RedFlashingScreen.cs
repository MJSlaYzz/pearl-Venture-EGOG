using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedFlashingScreen : MonoBehaviour
{
    [SerializeField] private OxygenSystem oxygenSystem;
    [SerializeField] private GameObject redScreen;
    [HideInInspector] private float oxygenLerpValue;

    [HideInInspector] public bool shouldFlash = false;

    [SerializeField] private float flashSpeed = 295f;

    [SerializeField] private float minFlashAlpha = 0.2f;
    [SerializeField] private float maxFlashAlpha = 0.4f;

    [SerializeField] private float minOxygenToFlash = 15f;

    [SerializeField] private PauseMenu pauseMenu; // can be removed.
    [SerializeField] private NoDeathMode noDeathMode;
    [SerializeField] public AudioSource playerHeartbeatsAudio;

    // make a value to stop the flash from happening in the first few secs/
    private float timer;
    private bool cantFlash = true;

    private void Start()
    {
        oxygenLerpValue = oxygenSystem.lerpValue;
    }
    // Update is called once per frame
    void Update()
    {
        oxygenLerpValue = oxygenSystem.lerpValue;
        CheckLerpValue();
        if (!cantFlash)
        {
            RedFlashTest();
        }
        TimeWithNoFlas();
    }
    void TimeWithNoFlas() //to prevent the screen from flashing at the begining of the game.
    {
        var maxTimer = 3f;
        if (cantFlash)
        {
            timer = Mathf.Clamp(timer, 0, maxTimer);
            timer += 1 * Time.deltaTime;
            playerHeartbeatsAudio.gameObject.SetActive(false);

            if (timer >= maxTimer)
            {
                cantFlash = false;
                playerHeartbeatsAudio.gameObject.SetActive(true);
            }
        }

    }
    void CheckLerpValue()
    {
        if (noDeathMode.noDeathMode == false)
        {
            //print("noDeathModeIsOn == false");
            if (oxygenLerpValue <= minOxygenToFlash)
            {
                shouldFlash = true;
            }
            else if (oxygenLerpValue > minOxygenToFlash || oxygenSystem.oxygenReplenisher)
            {
                shouldFlash = false;
                if (playerHeartbeatsAudio.isPlaying)
                {
                    playerHeartbeatsAudio.Stop();
                }

            }
        }
        else if (noDeathMode.noDeathMode == true)
        {
            //print("noDeathModeIsOn == true");
            shouldFlash = false;
            if (playerHeartbeatsAudio.isPlaying)
            {
                playerHeartbeatsAudio.Stop();
            }
        }

    }
    void RedFlashTest()
    {
        var color = redScreen.GetComponent<Image>().color;
        if (shouldFlash && !pauseMenu.pauseMenuUI.activeInHierarchy)
        {
            if(color.a == 0f || color.a == minFlashAlpha)
            {
                color.a = maxFlashAlpha;
                if (!playerHeartbeatsAudio.isPlaying)
                {
                    playerHeartbeatsAudio.Play();
                }
            }
            else
            {
                color.a -= 0.001f * flashSpeed * Time.deltaTime;
                if(color.a <= minFlashAlpha)
                {
                    color.a = minFlashAlpha;
                }
            }
        }
        else if (!shouldFlash)
        {
            if(color.a > 0f)
            {
                color.a -= 0.001f * flashSpeed;
                if (color.a <= 0f)
                {
                    color.a = 0f;
                }
            }
        }
        redScreen.GetComponent<Image>().color = color;
    }
}
