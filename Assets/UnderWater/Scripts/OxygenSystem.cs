using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class OxygenSystem : MonoBehaviour
{
    #region Variables

    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private Gradient oxygenGradient;
    [SerializeField] private Gradient jfOxygenGradient;
    [SerializeField] private Image oxygenFill;
    [SerializeField] private Text oxygenText;

    [SerializeField] private float maxOxygen = 50;
    [HideInInspector] public float lerpValue; // AKA Oxygen Value

    [HideInInspector] private float timer;
    [SerializeField] public float smoothTime = 50f; // the amount of time it takes for the oxygen to finish

    [HideInInspector] public bool shouldLerp = true;
    [HideInInspector] public bool oxygenReplenisher = false;
    [SerializeField] private float refillMultiplier = 10f; // the higher the number the faster the refill timer goes.

    [SerializeField] private PlayerHealth playerHealth;

    // Jelly Fish Variables.
    [HideInInspector] public bool jellyFishDebuff = false;
    [SerializeField] private float jellyFishDebuffStrength = 5f; // the higher the number the faster the player loses O2.

    #endregion

     void Start()
    {
        /*
        if(GameEvents.current != null)
        {
            GameEvents.current.jellyFishActionOn += jellyFishOn;
            GameEvents.current.jellyFishActionOff += jellyFishOff;
        }
        */
    }
    /*
     public void jellyFishOn()
     {
         jellyFishDebuff = true;
     }

    public void jellyFishOff()
     {
         // reset strength and effect
         jellyFishDebuff = false;
     }
    */
    void Update()
    {
        LerpDown();
        RefillOxygen();
        SetMaxOxygenValue();
        LerpDownFaster();
    }

    void LerpDown()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (shouldLerp && timer >= 0 && !jellyFishDebuff)
        {
            timer += 1 * Time.deltaTime;
            //Debug.Log("Timer 2 is " + timer);
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            //Debug.Log("inverseT 2 is " + inverseT);
            lerpValue = Mathf.Lerp(maxOxygen, 0, inverseT);
            //Debug.Log("lerpValue 2 is " + lerpValue);
        }
        if(lerpValue <= 0)
        {
            playerHealth.Dying();
        }
    }
    void LerpDownFaster() // instead of doing this we can just add the jellyFishEffectStrength to LerpDown() and change it's value.
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (shouldLerp && timer >= 0 && jellyFishDebuff)
        {
            timer += 1 * Time.deltaTime * jellyFishDebuffStrength;
            //Debug.Log("Timer 2 is " + timer);
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            //Debug.Log("inverseT 2 is " + inverseT);
            lerpValue = Mathf.Lerp(maxOxygen, 0, inverseT);
            //Debug.Log("lerpValue 2 is " + lerpValue);
        }
        if (lerpValue <= 0)
        {
            playerHealth.Dying();
        }
    }
    public void RefillOxygen()
    {
        //will be changed.
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (oxygenReplenisher == true) //Input.GetKey(KeyCode.M) || 
        {
            shouldLerp = false;
            timer += -1 * Time.deltaTime * refillMultiplier;
            //Debug.Log("Timer 1 is " + timer);
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            //Debug.Log("inverseT 1 is " + inverseT);
            lerpValue = Mathf.Lerp(maxOxygen, 0, inverseT);
            //Debug.Log("lerpValue 1 is " + lerpValue);
        }
        else
        {
            shouldLerp = true;
        }
    }
    public void InstantFullOxygen()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        shouldLerp = false;
        //timer += -1 * Time.deltaTime * 10000f;
        timer += -smoothTime; // just testing, if it didn't work use the one above.
        float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
        lerpValue = Mathf.Lerp(maxOxygen, 0, inverseT);
        //Debug.Log("lerpValue 1 is " + lerpValue);
        if(lerpValue == maxOxygen)
        {
            shouldLerp = true;
        }
    }
    void SetMaxOxygenValue()
    {
        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = lerpValue;
        oxygenText.text = lerpValue.ToString("00"); //we use "00" to show 2 digit numbers only.

        if (!jellyFishDebuff)
        {
            oxygenFill.color = oxygenGradient.Evaluate(oxygenSlider.normalizedValue);
        }
        else
        {
            oxygenFill.color = jfOxygenGradient.Evaluate(oxygenSlider.normalizedValue);
        }

    }
    /*
    void Lerp()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (Input.GetKey(KeyCode.M))
        {
            timer += 1 * Time.deltaTime;
            //Debug.Log("Timer 1 is " + timer);
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            //Debug.Log("inverseT 1 is " + inverseT);
            lerpValue = Mathf.Lerp(maxOxygen, 0, inverseT);
            Debug.Log("lerpValue 1 is " + lerpValue);

        }
        else if ((Input.GetKey(KeyCode.N)))
        {
            timer += -1 * Time.deltaTime;
            //Debug.Log("Timer 2 is " + timer);
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            //Debug.Log("inverseT 2 is " + inverseT);
            lerpValue = Mathf.Lerp(maxOxygen, 0, inverseT);
            Debug.Log("lerpValue 2 is " + lerpValue);


        }

    }
    /*
    /*
    void WorkingLerp()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (Input.GetKey(KeyCode.M))
        {
            timer += 1 * Time.deltaTime;
            //Debug.Log("Timer 1 is " + timer);
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            //Debug.Log("inverseT 1 is " + inverseT);
            lerpValue = Mathf.Lerp(0, maxOxygen, inverseT);
            Debug.Log("lerpValue 1 is " + lerpValue);

        }
        else if ((Input.GetKey(KeyCode.N)))
        {
            timer += -1 * Time.deltaTime;
            //Debug.Log("Timer 2 is " + timer);
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            //Debug.Log("inverseT 2 is " + inverseT);
            lerpValue = Mathf.Lerp(0, maxOxygen, inverseT);
            Debug.Log("lerpValue 2 is " + lerpValue);


        }
    

    }
    */
}
