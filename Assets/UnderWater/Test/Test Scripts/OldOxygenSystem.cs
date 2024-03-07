using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldOxygenSystem : MonoBehaviour
{
    [SerializeField] Text oxygenText;

    float minOxygen = 0f;
    [SerializeField] private float maxOxygen = 50f;
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private Gradient oxygenGradient;
    [SerializeField] private Image oxygenFill;

    private bool shouldLerp;

    float currentOxygen; // the cureent amount of oxygen, that should be changed every frame.
    float timeStartedLerping; // timeStartedLerping is "Now" or the moment the game starts, which is 0;
    [SerializeField] float lerpTime = 1; //the higher the number the slower the timer goes.

    // Start is called before the first frame update
    void Start()
    {
        timeStartedLerping = Time.time;
        //Debug.Log(timeStartedLerping);
        shouldLerp = true;
    }

    // Update is called once per frame
    void Update()
    {
        Oxygen();
        SetMaxOxygenValue();
    }
    void Oxygen()
    {
        if (shouldLerp)
        {
            currentOxygen = Lerp(maxOxygen, minOxygen, timeStartedLerping, lerpTime);
            //Debug.Log(currentOxygen);
            oxygenText.text = currentOxygen.ToString("00"); //shows 2 digits numbers
        }
        if (currentOxygen <= 0)
        {
            //Debug.Log("lerp is off");
            shouldLerp = false;
        }
    }
    void SetMaxOxygenValue()
    {
        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = currentOxygen;

        oxygenFill.color = oxygenGradient.Evaluate(oxygenSlider.normalizedValue);
    }
    public float Lerp(float star,float end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        //Debug.Log(timeSinceStarted);
        float percentageComplete = (timeSinceStarted / lerpTime);
        //Debug.Log(percentageComplete);
        var result = Mathf.Lerp(star, end, percentageComplete);
        return result;
    }
}
