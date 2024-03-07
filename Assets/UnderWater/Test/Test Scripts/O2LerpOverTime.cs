using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O2LerpOverTime : MonoBehaviour
{
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private Gradient oxygenGradient;
    [SerializeField] private Image oxygenFill;
    [SerializeField] private Text oxygenText;

    [SerializeField] private float maxOxygen = 50;
    [HideInInspector] private float lerpValue; // AKA Oxygen Value

    [HideInInspector] private float timer;
    [SerializeField] private float smoothTime = 50f; // the amount of time it takes for the oxygen to finish

    [HideInInspector] private bool shouldLerp = true;
    [SerializeField] private float refillMultiplier = 10f; // the higher the number the faster the refill timer goes.
    void Update()
    {
        LerpDown();
        RefillOxygen();
        SetMaxOxygenValue();
    }
    
    void LerpDown()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (shouldLerp && timer >= 0)
        {
            timer += 1 * Time.deltaTime;
            //Debug.Log("Timer 2 is " + timer);
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            //Debug.Log("inverseT 2 is " + inverseT);
            lerpValue = Mathf.Lerp(maxOxygen, 0, inverseT);
            //Debug.Log("lerpValue 2 is " + lerpValue);
        }
    }
    void RefillOxygen()
    {
        timer = Mathf.Clamp(timer, 0, smoothTime);
        if (Input.GetKey(KeyCode.M))
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
    void SetMaxOxygenValue()
    {
        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = lerpValue;
        oxygenText.text = lerpValue.ToString("00"); //we use "00" to show 2 digit numbers only.
        oxygenFill.color = oxygenGradient.Evaluate(oxygenSlider.normalizedValue);
    }
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
            //Debug.Log("lerpValue 1 is " + lerpValue);

        }
        else if ((Input.GetKey(KeyCode.N)))
        {
            timer += -1 * Time.deltaTime;
            //Debug.Log("Timer 2 is " + timer);
            float inverseT = Mathf.InverseLerp(0, smoothTime, timer);
            //Debug.Log("inverseT 2 is " + inverseT);
            lerpValue = Mathf.Lerp(maxOxygen, 0, inverseT);
            //Debug.Log("lerpValue 2 is " + lerpValue);


        }

    }
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
