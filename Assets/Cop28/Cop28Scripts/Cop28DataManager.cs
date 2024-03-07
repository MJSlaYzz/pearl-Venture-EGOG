using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cop28DataManager : MonoBehaviour
{
    public int currentLevelProgress { get; set; }
    private PlayerHealth playerHealth;
    public int progressLevel;
    public float progressPoints;
    public bool maxedPointsReached = false;
    public Slider progressSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI progressPointsText;
    public ChangeWaterColor changeWaterColor;
    public Cop28CollectablesCounter Cop28CollectablesCounter;
    [SerializeField] private bool inTutorial = false;
    public int pearlsCollected; // should be changed to add up the total amount of pearls collected to the total using +=

    // Update is called once per frame
    //private void Awake()
    //{
    //    UpdateWaterColor();
    //    UpdateLevelText();
    //}
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        UpdateWaterColor();
        UpdateLevelText();
        ProgressPointsCalculator(); //so other scripts can read the progressLevel real value in their start methods.
    }
    void Update()
    {
        //UpdateWaterColor(); //should be removed from update to awake or start. so that it would change when a new level loads.
        //UpdateLevelText(); //should be removed from update to awake or start. so that it would change when a new level loads.
        currentLevelProgress = progressLevel;
        progressSlider.value = progressPoints;
        ProgressPointsCalculator();
        if(progressPoints >= 300)
        {
            maxedPointsReached = true;
        }
        if (playerHealth != null && playerHealth.playerKnockedDown && progressPointsText.text != string.Empty)
        {
            progressPointsText.text = string.Empty;
        }
    }

    public void UpdateLevelText()
    {
        if (inTutorial)
        {
            levelText.text = "TUTORIAL LEVEL";
        }
        else
        {
            //Start Screen Should Give you info about the current state of the level.
            if (currentLevelProgress == 0)
            {
                levelText.text = "PROGRESS STAGE: POLLUTED";
                //The water is murky, the fish population is low, and there's a lot of trash and pollution.
            }
            else if (currentLevelProgress == 1)
            {
                levelText.text = "PROGRESS STAGE: RESTORING";
                //The water is starting to clear, some fish are returning, and trash is being removed.
            }
            else if (currentLevelProgress == 2)
            {
                levelText.text = "PROGRESS STAGE: THRIVING";
                //The water is clear, the fish population is increasing, and the sea is becoming more vibrant.
            }
            else if (currentLevelProgress == 3)
            {
                levelText.text = "PROGRESS STAGE: BLOSSOMING";
                //The water is crystal-clear, the fish population is thriving, and the sea is teeming with marine life.
            }
        }

    }
    public void UpdateWaterColor()
    {
        for (int i = 0; i < changeWaterColor.healthStages.Count; i++)
        {
            if (i == currentLevelProgress)
            {
                changeWaterColor.healthStages[i] = true;
            }
            else
            {
                changeWaterColor.healthStages[i] = false;
            }
        }
        /*
        if (currentLevelProgress == 0)
        {
            changeWaterColor.healthStages[1] = false;
            changeWaterColor.healthStages[2] = false;
            changeWaterColor.healthStages[3] = false;
            changeWaterColor.healthStages[0] = true;
        }
        else if (currentLevelProgress == 1)
        {
            changeWaterColor.healthStages[0] = false;
            changeWaterColor.healthStages[2] = false;
            changeWaterColor.healthStages[3] = false;
            changeWaterColor.healthStages[1] = true;
        }
        else if (currentLevelProgress == 2)
        {
            changeWaterColor.healthStages[0] = false;
            changeWaterColor.healthStages[1] = false;
            changeWaterColor.healthStages[3] = false;
            changeWaterColor.healthStages[2] = true;
        }
        else if (currentLevelProgress == 3)
        {
            changeWaterColor.healthStages[0] = false;
            changeWaterColor.healthStages[1] = false;
            changeWaterColor.healthStages[2] = false;
            changeWaterColor.healthStages[3] = true;
        }
        */
    }
    void ProgressPointsCalculator()
    {
        //print("progress Level = " + progressLevel);
        if(progressPoints < 100)
        {
            progressLevel = 0;
        }
        else if (progressPoints >= 100 && progressPoints < 200)
        {
            progressLevel = 1;
            //Debug.Log("progress Level 1 reaced!");
        }
        else if (progressPoints >= 200 && progressPoints < 300)
        {
            progressLevel = 2;
            //Debug.Log("progress Level 2 reaced!");
        }
        else if (progressPoints >= 300)
        {
            progressLevel = 3;
            //Debug.Log("progress Level 3 reaced!");
        }
    }
    public void AddPoints(int pointsAmount)
    {
        if(progressPoints < 300)
        {
            progressPoints += pointsAmount;
            Debug.Log(pointsAmount + " points were added!");
            progressPointsText.text = pointsAmount.ToString();
            progressPointsText.GetComponent<Animator>().SetTrigger("AddPoints");
            if(progressPoints > 300)
            {
                progressPoints = 300;
            }
        }
        else
        {
            Debug.Log(pointsAmount + " Max Progress Points were reached!");
        }

    }
    public void SaveData()
    {
        SaveTotalPearlsAfterLevel();
        Cop28SaveSystem.SaveProgress(this);
    }
    public void LoadData()
    {
        Cop28Data data = Cop28SaveSystem.LoadProgress();
        currentLevelProgress = data.progressLevel;
        progressPoints = data.progressPoints;
        pearlsCollected = data.totalPearlsCollected;
        maxedPointsReached = data.maxedPointsReached;
    }
    public void SaveTotalPearlsAfterLevel()
    {
        pearlsCollected += Cop28CollectablesCounter.totalPearlsForShare;
    }
}
