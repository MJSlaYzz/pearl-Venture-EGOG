using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [Header("Cop28 Variables")]
    [SerializeField] private bool isCop28 = false;
    [SerializeField] private float progressPoints;
    [SerializeField] private string progressStage;
    [SerializeField] private int progressLevel;
    [SerializeField] private int pearlsCollected;
    [SerializeField] private TextMeshProUGUI progressPointsText;
    [SerializeField] private TextMeshProUGUI progressStageText;
    [SerializeField] private TextMeshProUGUI pearlsCollectedText;
    [SerializeField] private ChangeWaterColor changeWaterColor;
    private void Start()
    {
        if (isCop28)
        {
            LoadData();
            ProgressPointsCalculator();
            ProgressStageCalculator();
            UpdateWaterColor();
            progressPointsText.text = progressPoints.ToString();
            progressStageText.text = progressStage;
            pearlsCollectedText.text = pearlsCollected.ToString();
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    #region Cop28
    void ProgressStageCalculator()
    {
        //print("progress Level = " + progressLevel);
        if (progressPoints < 100)
        {
            progressStage = "POLLUTED";
        }
        else if (progressPoints >= 100 && progressPoints < 200)
        {
            progressStage = "RESTORING";
        }
        else if (progressPoints >= 200 && progressPoints < 300)
        {
            progressStage = "THRIVING";
        }
        else if (progressPoints >= 300)
        {
            progressStage = "BLOSSOMING";
        }
    }
    void ProgressPointsCalculator()
    {
        //print("progress Level = " + progressLevel);
        if (progressPoints < 100)
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
    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/pearl.path"))
        {
            Cop28Data data = Cop28SaveSystem.LoadProgress();
            progressPoints = data.progressPoints;
            pearlsCollected = data.totalPearlsCollected;
        }
        else
        {
            Debug.Log("there's no saved file");
        }
    }
    public void UpdateWaterColor()
    {
        for (int i = 0; i < changeWaterColor.healthStages.Count; i++)
        {
            if (i == progressLevel)
            {
                changeWaterColor.healthStages[i] = true;
            }
            else
            {
                changeWaterColor.healthStages[i] = false;
            }
        }
    }
    #endregion

}
